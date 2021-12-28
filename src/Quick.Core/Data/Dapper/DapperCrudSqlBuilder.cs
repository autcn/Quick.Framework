using Quick;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dapper
{
    internal class DapperCrudSqlBuilder
    {
        #region Private fields
        private DapperCrudOptions _options;
        #endregion

        #region Constructors
        public DapperCrudSqlBuilder(DapperCrudOptions options)
        {
            _options = options;
        }
        #endregion

        #region Global Cache
        //缓存表别名
        private static readonly ConcurrentDictionary<Type, string> TableNames = new ConcurrentDictionary<Type, string>();

        //缓存列别名
        private static readonly ConcurrentDictionary<string, string> ColumnNames = new ConcurrentDictionary<string, string>();

        //缓存SQL构建
        private static readonly ConcurrentDictionary<string, string> StringBuilderCacheDict = new ConcurrentDictionary<string, string>();

        public void StringBuilderCache(StringBuilder sb, string cacheKey, Action<StringBuilder> stringBuilderAction)
        {
            if (_options.StringBuilderCacheEnabled && StringBuilderCacheDict.TryGetValue(cacheKey, out string value))
            {
                sb.Append(value);
                return;
            }

            StringBuilder newSb = new StringBuilder();
            stringBuilderAction(newSb);
            value = newSb.ToString();
            StringBuilderCacheDict.AddOrUpdate(cacheKey, value, (t, v) => value);
            sb.Append(value);
        }

        #endregion

        #region Get Names
        public string GetTableName(object entity)
        {
            var type = entity.GetType();
            return GetTableName(type);
        }

        public string GetTableName(Type type)
        {
            string tableName;
            //先查缓存
            if (TableNames.TryGetValue(type, out tableName))
                return tableName;
            //解析
            tableName = _options.TableNameResolver.ResolveTableName(type);
            //加入缓存
            TableNames.AddOrUpdate(type, tableName, (t, v) => tableName);
            return tableName;
        }

        public string GetColumnName(PropertyInfo propertyInfo)
        {
            //定义key
            string columnName, key = string.Format("{0}.{1}", propertyInfo.DeclaringType, propertyInfo.Name);

            //先查缓存
            if (ColumnNames.TryGetValue(key, out columnName))
                return columnName;

            //解析列名
            columnName = _options.ColumnNameResolver.ResolveColumnName(propertyInfo);

            //加入缓存
            ColumnNames.AddOrUpdate(key, columnName, (t, v) => columnName);

            return columnName;
        }
        #endregion

        #region Get Properties
        private IEnumerable<PropertyInfo> GetIdProperties(object entity)
        {
            var type = entity.GetType();
            return GetIdProperties(type);
        }

        public IEnumerable<PropertyInfo> GetIdProperties(Type type)
        {
            return type.GetProperties().Where(p => IsKey(p));
        }
        public IEnumerable<PropertyInfo> GetAllProperties(object entity)
        {
            if (entity == null) return new PropertyInfo[0];
            return entity.GetType().GetProperties();
        }

        //Get all properties that are not decorated with the Editable(false) attribute
        public IEnumerable<PropertyInfo> GetScaffoldableProperties(Type entityType)
        {
            IEnumerable<PropertyInfo> props = entityType.GetProperties()
                .Where(p => p.PropertyType.IsSimpleType() && !p.IsDefined(typeof(NotMappedAttribute)));

            return props;
        }

        private IEnumerable<PropertyInfo> GetInsertableProperties(Type entityType)
        {
            return GetScaffoldableProperties(entityType).Where(p =>
            {
                EditableAttribute editAttr = p.GetCustomAttribute<EditableAttribute>();
                if (editAttr == null)
                {
                    return true;
                }
                return editAttr.AllowEdit;
            });
        }

        /// <summary>
        /// 获取可以更新的列，非Id，非主键，非Editable(false)的可以更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetUpdateableProperties(Type entityType)
        {
            return GetScaffoldableProperties(entityType)
                .Where(p =>
                {
                    if (IsKey(p))
                    {
                        return false;
                    }
                    EditableAttribute editAttr = p.GetCustomAttribute<EditableAttribute>();
                    if (editAttr == null)
                    {
                        return true;
                    }
                    return editAttr.AllowEdit;
                });
        }
        #endregion

        #region Build Sqls
        public void AddObject(DynamicParameters dynParams, object obj, string[] fields, string suffix = "")
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (fields != null && !FieldsHasName(fields, prop.Name))
                {
                    continue;
                }
                dynParams.Add("@" + $"{prop.Name}{suffix}", prop.GetValue(obj));
            }
        }
        private bool IsKey(PropertyInfo prop)
        {
            return prop.IsDefined(typeof(KeyAttribute))
                || prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase);
        }
        private static bool FieldsHasName(string[] fields, string name)
        {
            if (fields == null)
            {
                return false;
            }
            return fields.Any(p => string.Compare(p, name, true) == 0);
        }
        private string GetFieldsKey(string[] fields)
        {
            if (fields == null)
            {
                return null;
            }
            return string.Join(",", fields);
        }
        public void BuildSelect(StringBuilder masterSb, IEnumerable<PropertyInfo> props, string[] fields, FieldsPolicy fieldsPolicy)
        {
            //bool isExcept = IsExcept(fields);
            StringBuilderCache(masterSb, $"{props.CacheKey()}_BuildSelect_{GetFieldsKey(fields) }_{fieldsPolicy}", sb =>
            {
                var propertyInfos = props as IList<PropertyInfo> ?? props.ToList();
                var addedAny = false;
                for (var i = 0; i < propertyInfos.Count(); i++)
                {
                    var property = propertyInfos.ElementAt(i);
                    if (fields != null && !IsKey(property)
                    )
                    {
                        if (
                           (fieldsPolicy == FieldsPolicy.Include && !FieldsHasName(fields, property.Name))
                        || (fieldsPolicy == FieldsPolicy.Exclude && FieldsHasName(fields, property.Name))
                        )
                            continue;
                    }

                    if (addedAny)
                        sb.Append(",");

                    sb.Append(GetColumnName(property));

                    //if there is a custom column name add an "as customcolumnname" to the item so it maps properly
                    ColumnAttribute colAttr = property.GetCustomAttribute<ColumnAttribute>();
                    if (colAttr != null && colAttr.Name != null)
                    {
                        sb.Append(" as " + string.Format(_options.Encapsulation, property.Name));
                    }

                    addedAny = true;
                }
            });
        }

        public void BuildWhere(Type type, StringBuilder sb, IEnumerable<PropertyInfo> whereProps, object whereConditions, string suffix = "")
        {
            var wherePropArray = whereProps.ToArray();
            var typeProperties = GetScaffoldableProperties(type).ToArray();
            for (var i = 0; i < wherePropArray.Length; i++)
            {
                var whereProp = wherePropArray[i];
                //用等号还是is null
                var useIsNull = false;
                //如果存在条件对象而且可读
                if (whereConditions != null && whereProp.CanRead)
                {
                    //如果值为空，则用is null
                    object obj = whereProp.GetValue(whereConditions, null);
                    if (obj == null || obj == DBNull.Value)
                        useIsNull = true;
                }
                //当where的类型和条件类型不一样时，需要翻译，以便从ColumnName拿数据库列名
                if (whereProp.PropertyType != type)
                {
                    whereProp = typeProperties.First(p => p.Name == whereProp.Name);
                }

                sb.AppendFormat(
                    useIsNull ? "{0} is null" : "{0} = @{1}" + suffix,
                    GetColumnName(whereProp),
                    whereProp.Name);

                if (i < wherePropArray.Length - 1)
                    sb.AppendFormat(" and ");
            }
        }

        private IEnumerable<PropertyInfo> GetBuildInsertProperties(Type type, string[] fields, FieldsPolicy fieldsPolicy, bool ignoreKey)
        {
            var props = GetInsertableProperties(type);
            IEnumerable<PropertyInfo> idProperties = null;
            if (ignoreKey)
            {
                idProperties = GetIdProperties(type);
            }

            return props.Where(property =>
            {
                if (fields != null)
                {
                    if (fieldsPolicy == FieldsPolicy.Include)
                    {
                        if (!FieldsHasName(fields, property.Name))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (FieldsHasName(fields, property.Name))
                        {
                            return false;
                        }
                    }
                }
                //排除自增Id
                if (idProperties != null && idProperties.Any(p => p.Name == property.Name))
                {
                    return false;
                }
                return true;
            });
        }
        public void BuildInsertValues(Type type, StringBuilder masterSb, string[] fields, FieldsPolicy fieldsPolicy, bool ignoreKey)
        {
            StringBuilderCache(masterSb, $"{type.FullName}_BuildInsertValues_{GetFieldsKey(fields)}_{fieldsPolicy}_{ignoreKey}", sb =>
            {
                var props = GetBuildInsertProperties(type, fields, fieldsPolicy, ignoreKey).ToList();
                for (int i = 0; i < props.Count; i++)
                {
                    sb.AppendFormat("@{0}", props[i].Name);
                    if (i < props.Count - 1)
                        sb.Append(", ");
                }
            });
        }

        public void BuildInsertParameters(Type type, StringBuilder masterSb, string[] fields, FieldsPolicy fieldsPolicy, bool ignoreKey)
        {
            StringBuilderCache(masterSb, $"{type.FullName}_BuildInsertParameters_{GetFieldsKey(fields) }_{fieldsPolicy}_{ignoreKey}", sb =>
            {
                var props = GetBuildInsertProperties(type, fields, fieldsPolicy, ignoreKey).ToList();
                for (int i = 0; i < props.Count; i++)
                {
                    sb.Append(GetColumnName(props[i]));
                    if (i < props.Count - 1)
                        sb.Append(", ");
                }
            });
        }
        public void BuildUpdateSet(Type type, StringBuilder masterSb, string[] fields, FieldsPolicy fieldsPolicy, bool updateKey)
        {
            StringBuilderCache(masterSb, $"{type.FullName}_BuildUpdateSet_{GetFieldsKey(fields)}_{fieldsPolicy}_{updateKey}", sb =>
            {
                var nonIdProps = updateKey ? GetInsertableProperties(type) : GetUpdateableProperties(type);
                var finalProps = nonIdProps.Where(property =>
                {
                    if (fields != null)
                    {
                        if (fieldsPolicy == FieldsPolicy.Include)
                        {
                            if (!FieldsHasName(fields, property.Name))
                                return false;
                        }
                        else
                        {
                            if (FieldsHasName(fields, property.Name))
                                return false;
                        }
                    }
                    return true;
                }).ToArray();
                for (var i = 0; i < finalProps.Length; i++)
                {
                    var property = finalProps[i];
                    sb.AppendFormat("{0} = @{1}", GetColumnName(property), property.Name);
                    if (i < finalProps.Length - 1)
                        sb.AppendFormat(", ");
                }
            });
        }

        #endregion
    }
}
