using Auto_Parts_Store.Models;
using System.Data;
using System.Threading.Tasks;

namespace Auto_Parts_Store.Repositories
{
    public interface IPersonRepository
    {
        Task<DataTable> GetAllPersonsAsync(PersonType type);
        Task<DataTable> SearchPersonsAsync(PersonType type, string searchText);
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id, decimal balance);
    }
}