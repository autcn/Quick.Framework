using System.Collections.Generic;

namespace Quick
{
    [QConfigurationSection(QProperties.DatabaseConfigName)]
    public class DatabaseConfiguration : Dictionary<string, string>
    {
    }
}
