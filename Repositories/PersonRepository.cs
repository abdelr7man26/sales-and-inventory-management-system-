using Auto_Parts_Store.Models;
using Auto_Parts_Store.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public async Task<DataTable> GetAllPersonsAsync(PersonType type)
        {
            string tableName = type == PersonType.Supplier ? "supplieres" : "customers";
            string query = $@"SELECT p.ID, p.PersonName AS [الأسم], p.Phone AS [التليفون], 
                             p.address AS [العنوان], t.Balance AS [الرصيد]
                             FROM person p
                             INNER JOIN {tableName} t ON p.ID = t.ID
                             WHERE p.isDeleted = 0";

            return await DbHelper.ExecuteQueryAsync(query);
        }

        public async Task<DataTable> SearchPersonsAsync(PersonType type, string searchText)
        {
            string tableName = type == PersonType.Supplier ? "supplieres" : "customers";
            string query = $@"SELECT p.ID, p.PersonName AS [الأسم], p.Phone AS [التليفون], 
                             p.address AS [العنوان], t.Balance AS [الرصيد]
                             FROM person p
                             INNER JOIN {tableName} t ON p.ID = t.ID
                             WHERE p.isDeleted = 0 AND (p.PersonName LIKE @search OR p.Phone LIKE @search)";

            return await DbHelper.ExecuteQueryAsync(query, new SqlParameter("@search", "%" + searchText + "%"));
        }

        public async Task AddPersonAsync(Person person)
        {
            using (SqlConnection con = DbHelper.GetConnection())
            {
                await con.OpenAsync();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string personQuery = @"INSERT INTO person (PersonName, Phone, address, isDeleted) 
                                         VALUES (@name, @phone, @addr, 0); SELECT SCOPE_IDENTITY();";

                    int newId = Convert.ToInt32(await DbHelper.ExecuteScalarWithTransactionAsync(personQuery, con, trans,
                        new SqlParameter("@name", person.PersonName),
                        new SqlParameter("@phone", (object)person.Phone ?? DBNull.Value),
                        new SqlParameter("@addr", (object)person.Address ?? DBNull.Value)));

                    string subTable = person.Type == PersonType.Supplier ? "supplieres" : "customers";
                    string subQuery = $"INSERT INTO {subTable} (ID, Balance) VALUES (@id, 0)";

                    await DbHelper.ExecuteNonQueryWithTransactionAsync(subQuery, con, trans, new SqlParameter("@id", newId));

                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdatePersonAsync(Person person)
        {
            string query = @"UPDATE person SET PersonName = @name, Phone = @phone, address = @addr WHERE ID = @id";
            await DbHelper.ExecuteNonQueryAsync(query,
                new SqlParameter("@name", person.PersonName),
                new SqlParameter("@phone", person.Phone),
                new SqlParameter("@addr", person.Address),
                new SqlParameter("@id", person.ID));
        }

        public async Task DeletePersonAsync(int id, decimal balance)
        {
            if (balance != 0)
                throw new Exception("لا يمكن حذف شخص رصيده غير مصفّر.");

            string query = "UPDATE person SET isDeleted = 1 WHERE ID = @id";
            await DbHelper.ExecuteNonQueryAsync(query, new SqlParameter("@id", id));
        }
    }
}