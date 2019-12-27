using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Solstice.Api.Helpers;
using Solstice.API.models;

namespace Solstice.API.DATA
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ContactRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T> Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Contact> Delete(Contact entity)
        {
            _context.PhoneNumbers.RemoveRange(entity.PhoneNumbers);
            await _context.SaveChangesAsync();

            _context.Entry(entity).State = EntityState.Deleted;
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task<Contact> GetContactRecord(int Id)
        {
            return await _context.Contacts.Include(c => c.PhoneNumbers)
            .FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<Contact> UpdateContactRecord(int Id, Contact contact)
        {
            try
            {
                var origin = await _context.Set<Contact>().Include(x => x.PhoneNumbers)
                    .Where(x => x.Id == Id).FirstOrDefaultAsync();
                var oldPhones = origin.PhoneNumbers;
                var newPhones = contact.PhoneNumbers;
                if (origin != null)
                {
                    // Update Phone Numbers
                    await UpdatePhoneNumbersCollection(oldPhones, newPhones);

                    origin.Address = contact.Address;
                    origin.Birthdate = contact.Birthdate;
                    origin.Company = contact.Company;
                    origin.Email = contact.Email;
                    origin.Name = contact.Name;
                    origin.ProfileImage = contact.ProfileImage;
                    await _context.SaveChangesAsync();
                    return contact;
                }
                else return null;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            
        }

        private async Task UpdatePhoneNumbersCollection(IEnumerable<PhoneNumber> oldList, IEnumerable<PhoneNumber> newList)
        {
            var IdsToRemove = oldList.Select(c => c.Id).Except(newList.Select(c => c.Id));
            var IdsToAdd = newList.Select(c => c.Id).Except(oldList.Select(c => c.Id));
            var IdsToUpdate = newList.Select(c => c.Id).Intersect(oldList.Select(c => c.Id));

            //remove
            await _context.PhoneNumbers.Where(x => IdsToRemove.Contains(x.Id)).ForEachAsync(x => _context.Remove(x));
            await _context.SaveChangesAsync();

            //add
            foreach (var phone in newList.Where(x =>IdsToAdd.Contains(x.Id)))
            {
                await _context.PhoneNumbers.AddAsync(phone);
            }
            await _context.SaveChangesAsync();

            //update
            foreach (var phone in newList.Where(x =>IdsToUpdate.Contains(x.Id)))
            {
                var phoneToUpdate = await _context.PhoneNumbers.FindAsync(phone.Id);
                phoneToUpdate = phone;
                
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Contact>> GetAllContacts()
        {
            return await _context.Contacts.Include(c => c.PhoneNumbers).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Contact>> GetContactsfiltered(contactParams contactParams)
        {
            var contacts = _context.Contacts.Include(c => c.PhoneNumbers)
            .AsQueryable();

            if (!string.IsNullOrEmpty(contactParams.PhoneNumber))
            {
                contacts = (from c in contacts
                               join p in _context.PhoneNumbers on c.Id equals p.ContactId
                               where p.Number == contactParams.PhoneNumber
                               select c).Include(c => c.PhoneNumbers).AsQueryable();
            }

            if (!string.IsNullOrEmpty(contactParams.Email))
            {
                contacts = contacts.Where(c => c.Email == contactParams.Email);
            }


            if (!string.IsNullOrEmpty(contactParams.City))
            {
                contacts = contacts.Where(c => c.Address.City == contactParams.City);
            }

            if (!string.IsNullOrEmpty(contactParams.State))
            {
                contacts = contacts.Where(c => c.Address.State == contactParams.State);
            }

            if (!string.IsNullOrEmpty(contactParams.Firstname))
            {
                contacts = contacts.Where(c => c.Name.FirstName == contactParams.Firstname);
            }

            if (!string.IsNullOrEmpty(contactParams.Lastname))
            {
                contacts = contacts.Where(c => c.Name.LastName == contactParams.Lastname);
            }

            return await contacts.ToListAsync();
            }
    }
}