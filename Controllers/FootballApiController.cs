using System.Threading.Tasks;
using AutoMapper;
using FootbalDataAPI.DATA;
using FootbalDataAPI.models;
using Microsoft.AspNetCore.Mvc;

namespace FootbalDataAPI.Controllers
{
    
    [Route("api/[controller]")]
     [ApiController]
    public class FootballApiController: ControllerBase
    {
        private readonly IFootballDataRepository _repo;
        private readonly IFootBallApiService _api;
        private readonly IMapper _mapper;
        public FootballApiController(IFootballDataRepository repo, IMapper mapper, IFootBallApiService service)
        {
            _mapper = mapper;
            _repo = repo;
            _api = service;
        }

         [HttpGet("import-league/{leagueCode}")]
        public async Task<IActionResult> ImportLeague(int leagueCode)
        {
            if (await _repo.CompetitionExists(leagueCode))
            {
                
            }
            var competitionAndTeam = _api.GetCompetitionAndTeams(leagueCode);
            
            var newCompetition = _mapper.Map<Competition>(competitionAndTeam.Competition);
            await _repo.Add(newCompetition);

            foreach (var team in competitionAndTeam.Teams)
            {
                var completeTeam = _api.GetCompleteTeamInfo(team.Id);
                if (true)
                {
                    
                }
                var newTeam = _mapper.Map<Team>(team);
                await _repo.Add(newTeam);

                foreach (var player in completeTeam.Squad)
                {
                    var newPlayer = _mapper.Map<Player>(player);
                    await _repo.Add(newPlayer);
                }
            }

            return Ok();
        }
    }
}