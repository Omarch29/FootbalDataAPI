using System.Collections.Generic;

namespace Solstice.API.DTOs
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string ProfileImage { get; set; }

        public string Forename { get; set; }

        public string Email { get; set; }

        public string Birthdate { get; set; }

        public string Address{ get; set; }

        public IList<PhoneNumberDTO> PhoneNumbers { get; set; }
    }


    public class PhoneNumberDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
    }
}