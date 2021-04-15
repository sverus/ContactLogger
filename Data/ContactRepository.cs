
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Data
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactContext _context;
        private readonly ILogger<ContactRepository> _logger;

        public ContactRepository(ContactContext context, ILogger<ContactRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Student[]> GetAllStudentsAsync(bool includeContacts = false)
        {
            _logger.LogInformation($"Getting all Students");

            IQueryable<Student> query = _context.Students;


            if (includeContacts)
            {
                query = query
                  .Include(c => c.Contacts);
            }

            return await query.ToArrayAsync();
        }

        public async Task<Student> GetStudentAsync(string moniker, bool includeContacts = false)
        {
            _logger.LogInformation($"Getting a Student for {moniker}");

            IQueryable<Student> query = _context.Students;

            //query = query.Where(c => c.Moniker == moniker);

            if (includeContacts)
            {
                query = query
                  .Include(c => c.Contacts);
            }
            // Query It
            query = query.Where(c => c.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ContactLog[]> GetContactsByMonikerAsync(string moniker)
        {
            _logger.LogInformation($"Getting all Contacts for a Student");

            IQueryable<ContactLog> query = _context.ContactLogs;

            query = query
            .Where(t => t.Student.Moniker == moniker);

            return await query.ToArrayAsync();

        }

        public async Task<ContactLog> GetContactByMonikerAsync(string moniker, int Id)
        {
            _logger.LogInformation($"Getting single Contact for a Student");

            IQueryable<ContactLog> query = _context.ContactLogs;

            query = query
  .Where(t => t.Id == Id && t.Student.Moniker == moniker);

            return await query.FirstOrDefaultAsync();
        }


    }
}
