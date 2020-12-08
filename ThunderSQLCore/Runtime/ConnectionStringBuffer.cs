using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ThunderSQLCore.Runtime
{
    public class ConnectionStringBuffer
    {
        #region Singleton

        private static ConnectionStringBuffer _instance;

        static ConnectionStringBuffer()
        {
            _instance = new ConnectionStringBuffer();
        }

        public static ConnectionStringBuffer Instance
        {
            get { return _instance; }
        }

        #endregion

        private Dictionary<string, ConnectionStringSettings> _buffer;
        private object _syncRoot = new object();

        public ConnectionStringBuffer()
        {
            _buffer = new Dictionary<string, ConnectionStringSettings>();
        }

        public ConnectionStringSettings Get(string connectionStringName)
        {
            if (!_buffer.ContainsKey(connectionStringName))
            {
                lock (_syncRoot)
                {
                    if (!_buffer.ContainsKey(connectionStringName))
                    {
                        _buffer.Add(connectionStringName, GetFromConfig(connectionStringName));
                    }
                }
            }

            return _buffer[connectionStringName];
        }

        private ConnectionStringSettings GetFromConfig(string connectionName)
        {
            //var setting = ConfigurationManager.ConnectionStrings[connectionName];

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("setting.json");
            var config = builder.Build();

            var connConfig = config.GetSection("ConnectionStrings:" + connectionName);
            if (connConfig == null)
            {
                var exceptionMessage = String.Concat("ConnectionStrings not found in config file 'setting.json'");
                throw new ThunderException(exceptionMessage);
            }

            var Name = connectionName; // 参数1：连接名称
            var Provider = connConfig.GetSection("providerName").Value;             // 参数2：处理方式
            if (string.IsNullOrWhiteSpace(Provider))
            {
                var exceptionMessage = String.Concat("ConnectionString '", connectionName, "'not config ‘providerName’");
                throw new ThunderException(exceptionMessage);
            }
            var ConnectionStr = connConfig.GetSection("connectionString").Value;    // 参数3：连接字符串
            if (string.IsNullOrWhiteSpace(ConnectionStr))
            {
                var exceptionMessage = String.Concat("ConnectionString '", connectionName, "' not config 'connectionString'");
                throw new ThunderException(exceptionMessage);
            }

            return new ConnectionStringSettings(Name, ConnectionStr, Provider);
        }
    }
}
