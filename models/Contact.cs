using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Solstice.API.models
{
    public class Contact
    {
        public int Id { get; set; }
        public string ProfileImage { get; set; }
        public Name Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        // Complex Property
        public Address Address { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public Contact()
        {
            PhoneNumbers = new Collection<PhoneNumber>();
        }
    }

    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Forename() {
            return $"{FirstName} {LastName}";
        }       
    }

}