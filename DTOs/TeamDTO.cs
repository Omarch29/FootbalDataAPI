using System.Collections.Generic;

namespace FootbalDataAPI.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tla { get; set; }
        public string ShortName { get; set; }
        public AreaDTO Area { get; set; }
        public string Email { get; set; }
       
    }
}