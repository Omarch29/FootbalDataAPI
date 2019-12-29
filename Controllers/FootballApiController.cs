using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FootbalDataAPI.DATA;
using FootbalDataAPI.models;
using Microsoft.AspNetCore.Mvc;

namespace FootbalDataAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FootballApiController : ControllerBase
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
        public async Task<IActionResult> ImportLeague(string leagueCode)
        {
            //try
            //{
                if (await _repo.CompetitionExists(leagueCode))
                {
                    return StatusCode(409, new { message = "League already imported" });
                }
                var competitionAndTeam = _api.GetCompetitionAndTeams(leagueCode);
                if (competitionAndTeam == null)
                {
                    return StatusCode(404, new { message = "Not found" });
                }

                var newCompetition = _mapper.Map<Competition>(competitionAndTeam.Competition);
                await _repo.Add(newCompetition);

                foreach (var team in competitionAndTeam.Teams)
                {
                    var completeTeam = _api.GetCompleteTeamInfo(team.Id);
                    if (!await _repo.CheckIfEntityExistsByEntityId<Team>(x => x.Id == team.Id))
                    {
                        var newTeam = _mapper.Map<Team>(team);
                        await _repo.Add(newTeam);
                        var teamComptetion = new CompetitionTeam()
                        {
                            CompetitionId = newCompetition.Id,
                            TeamId = newTeam.Id,
                        };
                        await _repo.Add(teamComptetion);

                        foreach (var player in completeTeam.Squad.Where(x => x.Role == "PLAYER"))
                        {
                            var newPlayer = _mapper.Map<Player>(player);
                            if (!await _repo.CheckIfEntityExistsByEntityId<Player>(x => x.Id == player.Id))
                            {
                                await _repo.Add(newPlayer);
                                var teamPlayer = new TeamPlayer()
                                {
                                    TeamId = team.Id,
                                    PlayerId = player.Id,
                                };
                                await _repo.Add(teamPlayer);
                            }
                        }
                    }
                }
                return StatusCode(201, new { message = "Successfully imported" });
            /*
            }
            
            catch (System.Exception)
            {

                return StatusCode(504, new { message = "Server Error" });
            }
            */
        }
    }
}