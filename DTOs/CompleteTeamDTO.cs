using System.Collections.Generic;

namespace FootbalDataAPI.DTOs
{
    public class CompleteTeamDTO: TeamDTO
    {
        public CompleteTeamDTO()
        {
            ActiveCompetitions = new List<CompetitionDTO>();
            Squad = new List<PlayerDTO>();
        }
        public List<CompetitionDTO> ActiveCompetitions { get; set; }
        public List<PlayerDTO> Squad { get; set; }
    }
}