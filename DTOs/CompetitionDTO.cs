using System.Collections.Generic;

namespace FootbalDataAPI.DTOs
{
    public class CompetitionDTO
    {
        public CompetitionDTO()
        {
            Teams = new List<TeamDTO>();
        }

        public int Id { get; set; }
        public AreaDTO Area { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public IList<TeamDTO> Teams { get; set; }
    }
}