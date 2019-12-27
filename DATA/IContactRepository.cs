using System.Collections.Generic;
using System.Threading.Tasks;
using Solstice.Api.Helpers;
using Solstice.API.models;

namespace Solstice.API.DATA
{
    public interface IContactRepository
    {
        Task<T> Add<T>(T entity) where T : class;
        Task<Contact> Delete(Contact entity);

        Task<Contact> GetContactRecord(int Id);

        Task<Contact> UpdateContactRecord(int Id, Contact contact);

        Task<IList<Contact>> GetAllContacts();

        Task<IList<Contact>> GetContactsfiltered(contactParams contactParams);
        
         Task<bool> SaveAll();
    }
}