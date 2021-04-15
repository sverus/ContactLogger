
using System.Threading.Tasks;

namespace ContactLogger.Data
{
    public interface IContactRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Student[]> GetAllStudentsAsync(bool includeContacts = false);
        Task<ContactLog[]> GetContactsByMonikerAsync(string moniker);
        Task<ContactLog> GetContactByMonikerAsync(string moniker, int id);
        Task<Student> GetStudentAsync(string moniker, bool includeContacts = false);
        Task<bool> SaveChangesAsync();
    }
}