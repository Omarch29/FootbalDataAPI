using System.Collections.Generic;
using FootbalDataAPI.DTOs;

namespace FootbalDataAPI.DATA
{
    public interface IFootBallApiService
    {
         CompetitionDTO GetCompetition(int Id);
         CompetitionAndTeamsDTO GetCompetitionAndTeams(string legueCode);
         List<TeamDTO> GetTeams(int CompetitionId);
         CompleteTeamDTO GetCompleteTeamInfo(int TeamId);
         
    }
}