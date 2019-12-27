namespace Solstice.API.models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string Number { get; set; }
        public bool IsPersonal { get; set; }

        public Contact Contact { get; set; }

        public override string ToString()
        {
            return $"{WorkOrPersonal()}: {Number}";
        }

        public string WorkOrPersonal()
        {
            return IsPersonal ? "Personal" : "Work";
        }
    }
}