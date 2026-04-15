using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Helpers
{
    public static class DbHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        public static async Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
        }
                }
            }

        public static async Task ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task<object> ExecuteScalarAsync(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                await con.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }
        public static async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static async Task<int> ExecuteNonQueryWithTransactionAsync(
            string query,
            SqlConnection con,
            SqlTransaction trans,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public static async Task<object> ExecuteScalarWithTransactionAsync(
            string query,
            SqlConnection con,
            SqlTransaction trans,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return await cmd.ExecuteScalarAsync();
            }
        }
    }
}