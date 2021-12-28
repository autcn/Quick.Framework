using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dapper
{
    public interface IColumnNameResolver
    {
        string ResolveColumnName(PropertyInfo propertyInfo);
    }
}
