using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using ThunderSQLCore.Provider.Common;

namespace ThunderSQLCore.Provider
{
    public class ProviderFactory
    {
        private const string MySql_Factory = "MySql.Data.MySqlClient";

        private const string SqlServer_Factory = "System.Data.SqlClient";

        //private const string Oracle_Factory = ""; //暂不支持Oracle

        public static Func<string, IDataProvider> CustomProvider { get; set; }

        public static Func<string, IDbConnection> ConnectionFactory { get; set; }

        private static Dictionary<string, Type> Providers;

        static ProviderFactory()
        {
            Providers = new Dictionary<string, Type>();
            AddProvider(SqlServer_Factory, typeof(SqlProvider));
            //AddProvider("System.Data.OracleClient", typeof(OracleProvider));
            AddProvider(MySql_Factory, typeof(MySqlProvider));
        }

        public IDataProvider Create(ConnectionStringSettings settings, Transaction transactionMode)
        {
            var provider = ResolveDataProvider(settings.ProviderName);
            provider.DbConnection = CreateConnection(settings.ProviderName, settings);
            provider.TransactionMode = transactionMode;
            return provider;
        }

        public IDataProvider ResolveDataProvider(string providerName)
        {
            if (CustomProvider != null) return CustomProvider(providerName);

            if (Providers.ContainsKey(providerName))
            {
                return (IDataProvider)Activator.CreateInstance(Providers[providerName]);
            }

            var exceptionMessage = String.Format(
                "ThunderSQLCore do not supports the '{0}' provider. Either add your provider using the ProviderFactory.AddProvider() method, or create and set a ProviderResolver.CustomProvider.",
                providerName);

            throw new ThunderException(exceptionMessage);
        }

        private IDbConnection CreateConnection(string providerName, ConnectionStringSettings settings)
        {
            IDbConnection connection;

            if (ConnectionFactory != null)
                connection = ConnectionFactory(providerName);
            else
            {
                switch (providerName)
                {
                    default:
                    case SqlServer_Factory:
                        var sqlserver = System.Data.SqlClient.SqlClientFactory.Instance;
                        connection = sqlserver.CreateConnection();
                        connection.ConnectionString = settings.ConnectionString;
                        break;
                    case MySql_Factory:
                        var mysql = MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                        connection = mysql.CreateConnection();
                        connection.ConnectionString = settings.ConnectionString;
                        break;
                }
            }

            return connection;
        }

        public static void AddProvider(string providerName, Type providerType)
        {
            if (String.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentException("Parameter 'providerName' can not be null and must contain data.", "providerName");
            }
            if (providerType == null)
            {
                throw new ArgumentNullException("providerType");
            }
            if (Providers.ContainsKey(providerName))
            {
                if (!Providers[providerName].Equals(providerType))
                {
                    var message = String.Format(
                        "Attempting to add a provider named '{0}' of type '{1}', but an existing provider of type '{2}' already exists with that name.",
                        providerName, providerType.ToString(), Providers[providerName].ToString());

                    throw new ThunderException(message);
                }
            }
            else
            {
                Providers.Add(providerName, providerType);
            }
        }
    }
}
