using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;

namespace ThunderSQLCore.Provider
{
    public interface IDataProvider : IDisposable
    {
        void CreateConnection(ConnectionStringSettings settings, Transaction transaction);

        IDbConnection DbConnection { get; set; }

        IDbTransaction DbTransaction { get; set; }

        Transaction TransactionMode { get; set; }

        string ParameterIdentifier { get; }

        string FieldFormat { get; }

        IDataReader Query(string query, object[] queryParams);

        int Execute(string command, object[] commandParams);

        int ExecuteGetIdentity(string command, object[] commandParams);

        object ExecuteGetValue(string query, object[] queryParams);

        string SelectAllQuery(string projection, string where);

        string SelectTakeQuery(string projection, string where, int count);

        void Commit();

        DataTable GetSchema();

        ConnectionState Status();
    }
}
