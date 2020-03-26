using System;
using System.Collections.Generic;
using System.Text;
using Dal.Contract.Interafces;
using System.Data;
using System.Data.SqlClient;

namespace Dal.Implementation
{
    public class ToDatabaseSaver : ISaver<IEnumerable<Tuple<string, int, double>>>
    {
        private readonly string connectionString;

        public ToDatabaseSaver(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Save(IEnumerable<Tuple<string, int, double>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sqlConnection = new SqlConnection(connectionString);
            var sqlCommand = new SqlCommand("InsertTrade", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            using (sqlConnection)
            {
                sqlConnection.Open();
                foreach (var trade in source)
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@currencyCodes", SqlDbType.NChar, 6));
                    sqlCommand.Parameters["@currencyCodes"].Value = trade.Item1;
                    sqlCommand.Parameters.Add(new SqlParameter("@numberOfTrades", SqlDbType.Int, 4));
                    sqlCommand.Parameters["@numberOfTrades"].Value = trade.Item2;
                    sqlCommand.Parameters.Add(new SqlParameter("@tradePrice", SqlDbType.Float));
                    sqlCommand.Parameters["@tradePrice"].Value = trade.Item3;

                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Parameters.Clear();
                }
            }
        }
    }
}
