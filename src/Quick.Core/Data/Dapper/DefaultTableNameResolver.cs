using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Dapper
{
    internal class DefaultTableNameResolver : ITableNameResolver
    {
        private readonly DapperCrudOptions _options;
        public DefaultTableNameResolver(DapperCrudOptions options)
        {
            _options = options;
        }
        public virtual string ResolveTableName(Type type)
        {
            var tableName = string.Format(_options.Encapsulation, type.Name);

            var tableAttr = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute), true);
            if (tableAttr != null)
            {
                tableName = string.Format(_options.Encapsulation, tableAttr.Name);
                try
                {
                    if (!String.IsNullOrEmpty(tableAttr.Schema))
                    {
                        string schemaName = string.Format(_options.Encapsulation, tableAttr.Schema); 
                        tableName = String.Format("{0}.{1}", schemaName, tableName);
                    }
                }
                catch
                {
                    //Schema doesn't exist on this attribute.
                }
            }

            return tableName;
        }
    }
}
