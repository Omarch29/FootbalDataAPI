namespace FootbalDataAPI.DTOs
{
    public class PlayerDTO
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Role { get; set; }
    }
}