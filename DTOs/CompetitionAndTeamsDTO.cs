using System.Collections.Generic;

namespace FootbalDataAPI.DTOs
{
    public class CompetitionAndTeamsDTO
    {
        public CompetitionAndTeamsDTO()
        {
            Teams = new List<TeamDTO>();
        }
        
        public CompetitionDTO Competition { get; set; }
        public IList<TeamDTO> Teams { get; set; }
    }
}