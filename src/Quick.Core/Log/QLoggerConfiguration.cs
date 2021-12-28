using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;

namespace Quick
{
    public enum LoggerSinkType
    {
        Console,
        File
    }

    [QConfigurationSection(QProperties.LogConfigName)]
    public class QLoggerConfiguration
    {
        public QLoggerConfiguration()
        {
            ConsoleLevel = LogEventLevel.Information;
            FileLevel = LogEventLevel.Information;
            OutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            FileName = "log-.txt";
        }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<LoggerSinkType> SinkTypes { get; set; }

        public string OutputTemplate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogEventLevel ConsoleLevel { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogEventLevel FileLevel { get; set; }



        #region File Settings

        [JsonConverter(typeof(StringEnumConverter))]
        public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;

        public string FileName { get; set; }
        public string LogStorageDir { get; set; }
        #endregion
    }
}
