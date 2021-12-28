using System;
using System.Collections.Generic;
using System.Text;

namespace Quick
{
    public class LocalStorageOptions
    {
        /// <summary>
        /// 存储于配置文件中，否则单独文件存储
        /// </summary>
        public bool UseConfigFile { get; set; }

        /// <summary>
        /// 单独文件存储时，文件路径
        /// </summary>
        public string SeparateFilePath { get; set; }
    }
}
