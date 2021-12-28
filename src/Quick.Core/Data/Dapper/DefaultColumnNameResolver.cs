using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Reflection;

namespace Dapper
{
    internal class DefaultColumnNameResolver : IColumnNameResolver
    {
        private readonly DapperCrudOptions _options;
        public DefaultColumnNameResolver(DapperCrudOptions options)
        {
            _options = options;
        }
        public virtual string ResolveColumnName(PropertyInfo propertyInfo)
        {
            var columnName = string.Format(_options.Encapsulation, propertyInfo.Name);

            var columnAttr = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute), true);
            if (columnAttr != null && columnAttr.Name != null)
            {
                columnName = string.Format(_options.Encapsulation, columnAttr.Name);
                if (Debugger.IsAttached)
                    Trace.WriteLine(String.Format("Column name for type overridden from {0} to {1}", propertyInfo.Name, columnName));
            }
            return columnName;
        }
    }
}
