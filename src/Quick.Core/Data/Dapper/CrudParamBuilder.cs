using System;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Dapper
{
    public enum FieldsPolicy
    {
        Include,
        Exclude
    }
    public class CrudBaseBuilder : CrudBaseBuilder<CrudBaseBuilder>
    {

    }
    public abstract class CrudBaseBuilder<T> where T : CrudBaseBuilder<T>
    {
        public FieldsPolicy FieldsPolicy { private set; get; } = FieldsPolicy.Include;
        public string[] Fields { private set; get; }
        public IDbTransaction DbTransaction { private set; get; }
        public int? CommandTimeout { private set; get; }
        public Type EntityType { private set; get; }
        public bool IsBuffered { private set; get; }
        public T SetTransaction(IDbTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
            return (T)this;
        }
        public T SetCommandTimeout(int? commandTimeout)
        {
            CommandTimeout = commandTimeout;
            return (T)this;
        }
        public T SetFields(params string[] fields)
        {
            Fields = fields;
            return (T)this;
        }
        public T SetFieldsPolicy(FieldsPolicy fieldsPolicy)
        {
            FieldsPolicy = fieldsPolicy;
            return (T)this;
        }
        public T SetEntityType(Type entityType)
        {
            EntityType = entityType;
            return (T)this;
        }
        public T SetIsBuffered(bool isBuffered)
        {
            IsBuffered = isBuffered;
            return (T)this;
        }
        public void Import(CrudBaseBuilder<T> builder)
        {
            SetFields(builder.Fields)
           .SetFieldsPolicy(builder.FieldsPolicy)
           .SetEntityType(builder.EntityType)
           .SetIsBuffered(builder.IsBuffered)
           .SetTransaction(builder.DbTransaction)
           .SetCommandTimeout(builder.CommandTimeout);
        }
    }
    public class CrudIdBuilder : CrudIdBuilder<CrudIdBuilder>
    {
        public CrudIdBuilder(object id) : base(id)
        {
        }
    }
    public class CrudIdTypeBuilder : CrudIdBuilder
    {
        public CrudIdTypeBuilder(Type entityType, object id) : base(id)
        {
            SetEntityType(entityType);
        }

    }
    public abstract class CrudIdBuilder<T> : CrudBaseBuilder<T> where T : CrudIdBuilder<T>
    {
        public object Id { get; private set; }
        public CrudIdBuilder(Type entityType, object id)
        {
            SetEntityType(entityType);
            Id = id;
        }
        public CrudIdBuilder(object id)
        {
            Id = id;
        }
        public T SetId(object id)
        {
            Id = id;
            return (T)this;
        }

        public void Import(CrudIdBuilder<T> builder)
        {
            base.Import(builder);
            SetId(builder.Id);
        }
    }

    public class CrudAppendSqlBuilder : CrudAppendSqlBuilder<CrudAppendSqlBuilder>
    {
    }
    public abstract class CrudAppendSqlBuilder<T> : CrudBaseBuilder<T> where T : CrudAppendSqlBuilder<T>
    {
        public string AppendSql { get; set; } = "";
        public T SetAppendSql(string appendSql)
        {
            if (appendSql == null)
            {
                AppendSql = "";
            }
            else
            {
                AppendSql = appendSql;
            }
            return (T)this;
        }

        public void Import(CrudAppendSqlBuilder<T> builder)
        {
            base.Import(builder);
            SetAppendSql(builder.AppendSql);
        }
    }

    public class CrudListBuilder : CrudListBuilder<CrudListBuilder>
    {
        public CrudListBuilder(object conditionObject) : base(conditionObject)
        {
        }
    }

    public class CrudListTypeBuilder : CrudListBuilder
    {
        public CrudListTypeBuilder(Type entityType, object conditionObject) : base(conditionObject)
        {
            SetEntityType(entityType);
        }
    }

    public abstract class CrudListBuilder<T> : CrudAppendSqlBuilder<T> where T : CrudListBuilder<T>
    {
        public object ConditionObject { get; private set; }
        public CrudListBuilder(Type entityType, object conditionObject)
        {
            SetEntityType(entityType);
            SetConditionObject(conditionObject);
        }
        public CrudListBuilder(object conditionObject)
        {
            SetConditionObject(conditionObject);
        }

        public T SetConditionObject(object conditionObject)
        {
            if (conditionObject != null && !conditionObject.GetType().IsClass)
            {
                throw new Exception("The conditionObject must be class");
            }
            ConditionObject = conditionObject;
            return (T)this;
        }
        public void Import(CrudListBuilder<T> builder)
        {
            base.Import(builder);
            SetConditionObject(builder.ConditionObject);
        }
    }
    public class CrudPagedQueryListBuilder : CrudPagedQueryListBuilder<CrudPagedQueryListBuilder>
    {
        public CrudPagedQueryListBuilder(object conditionObject, string orderBy)
            : base(conditionObject, orderBy)
        {
            SetConditionObject(conditionObject);
            SetOrderBy(orderBy);
        }
    }

    public class CrudPagedQueryListTypeBuilder : CrudPagedQueryListBuilder
    {
        public CrudPagedQueryListTypeBuilder(Type entityType, object conditionObject, string orderBy)
            : base(conditionObject, orderBy)
        {
            SetEntityType(entityType);
        }
    }
    public abstract class CrudPagedQueryListBuilder<T> : CrudListBuilder<T> where T : CrudPagedQueryListBuilder<T>
    {
        public int Page { get; private set; } = 1;
        public int RowsPerPage { get; set; } = 100000;
        public string OrderBy { get; set; }
        public CrudPagedQueryListBuilder(Type entityType, object conditionObject, string orderBy)
            : base(entityType, conditionObject)
        {
            SetEntityType(entityType);
            SetConditionObject(conditionObject);
            SetOrderBy(orderBy);
        }
        public CrudPagedQueryListBuilder(object conditionObject, string orderBy)
            : base(conditionObject)
        {
            SetConditionObject(conditionObject);
            SetOrderBy(orderBy);
        }

        public T SetPage(int page)
        {
            Page = page;
            return (T)this;
        }
        public T SetRowsPerPage(int rowsPerPage)
        {
            RowsPerPage = rowsPerPage;
            return (T)this;
        }
        public T SetOrderBy(string orderBy)
        {
            Trace.Assert(orderBy != null);
            OrderBy = orderBy;
            return (T)this;
        }
        public void Import(CrudPagedQueryListBuilder<T> builder)
        {
            base.Import(builder);
            SetPage(builder.Page)
           .SetRowsPerPage(builder.RowsPerPage)
           .SetOrderBy(builder.OrderBy);
        }
    }
    public class CrudEntityBuilder : CrudEntityBuilder<CrudEntityBuilder>
    {
        public CrudEntityBuilder(object entity) : base(entity)
        {
        }
    }
    public abstract class CrudEntityBuilder<T> : CrudBaseBuilder<T> where T : CrudEntityBuilder<T>
    {
        public object Entity { get; private set; }
        public CrudEntityBuilder(object entity)
        {
            SetEntity(entity);
        }

        public T SetEntity(object entity)
        {
            Entity = entity;
            SetEntityType(entity.GetType());
            return (T)this;
        }
    }

    public class CrudUpdateWhereBuilder : CrudUpdateWhereBuilder<CrudUpdateWhereBuilder>
    {
        public CrudUpdateWhereBuilder(object entity, object conditionObject) : base(entity, conditionObject)
        {
        }
    }
    public abstract class CrudUpdateWhereBuilder<T> : CrudListBuilder<T> where T : CrudUpdateWhereBuilder<T>
    {
        public object Entity { get; private set; }
        public CrudUpdateWhereBuilder(object entity, object conditionObject) : base(conditionObject)
        {
            SetEntity(entity);
        }

        public T SetEntity(object entity)
        {
            Entity = entity;
            SetEntityType(entity.GetType());
            return (T)this;
        }
    }
}
