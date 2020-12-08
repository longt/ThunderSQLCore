using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace ThunderSQLCore.Provider.Common
{
    public class MySqlProvider : DefaultProvider
    {
        public override DataTable GetSchema()
        {
            DataTable dt;
            using (MySqlConnection mysqlconnect = (MySqlConnection)base.DbConnection)
            {
                mysqlconnect.Open();
                dt = mysqlconnect.GetSchema("Tables");//获取架构
            }
            return dt;
        }
        public override int ExecuteGetIdentity(string command, object[] commandParams)
        {
            var identityQuery = String.Concat(command, "; SELECT last_insert_id();");
            var value = CreateDbCommand(identityQuery, commandParams).ExecuteScalar();
            return Convert.ToInt32(value);

            throw new NotImplementedException();
        }

        public override string FieldFormat
        {
            get { return "`{0}`"; }
        }

        public override string ParameterIdentifier
        {
            get { return "@"; }
        }

        public override string SelectAllQuery(string projection, string where)
        {
            return String.Format("SELECT {0} {1}", projection, where);
        }

        public override string SelectTakeQuery(string projection, string where, int count)
        {
            return String.Format("SELECT {0} {1} LIMIT {2}", projection, where, count);
        }
    }
}
