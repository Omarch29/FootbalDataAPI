using System.Collections.Generic;
using Solstice.API.models;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Solstice.API.DATA
{
    public class Seed
    {
        private DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;

        }

        public void SeedContacts()
        {
            if (_context.Contacts.Count() > 0) return;

            _context.PhoneNumbers.RemoveRange(_context.PhoneNumbers);
            _context.SaveChanges();

            _context.Contacts.RemoveRange(_context.Contacts);
            _context.SaveChanges();

            // seed contacts
            var contactData = System.IO.File.ReadAllText("Data/ContactRecordsData.json");
            var contacts = JsonConvert.DeserializeObject<List<Contact>>(contactData);
            foreach (var contact in contacts)
            {
                _context.Contacts.Add(contact);

            }
            _context.SaveChanges();
        }
    }
}