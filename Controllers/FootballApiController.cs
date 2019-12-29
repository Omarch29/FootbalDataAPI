using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FootbalDataAPI.DATA;
using FootbalDataAPI.models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FootbalDataAPI.Controllers
{
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
            try
            {
                //transform legueCode to Upper Case
                leagueCode = leagueCode.ToUpper();

                // Verify if the competition was already added
                if (await _repo.CompetitionExists(leagueCode))
                {
                    return StatusCode(409, new { message = "League already imported" });
                }

                //Get the Competition and the teams from the API
                var competitionAndTeam = _api.GetCompetitionAndTeams(leagueCode);
                if (competitionAndTeam == null)
                {
                    return StatusCode(404, new { message = "Not found" });
                }

                //Map and Add the new competition
                var newCompetition = _mapper.Map<Competition>(competitionAndTeam.Competition);
                await _repo.Add(newCompetition);

                //Foreach Team from API
                foreach (var team in competitionAndTeam.Teams)
                {
                    //Check if the team was already created for another competition                 
                    if (!await _repo.CheckIfEntityExistsByEntityId<Team>(x => x.Id == team.Id))
                    {

                        //map the team and Added
                        var newTeam = _mapper.Map<Team>(team);
                        await _repo.Add(newTeam);

                        //Get the full Squad from the Team
                        var completeTeam = _api.GetCompleteTeamInfo(team.Id);
                       
                        //For Each Player (ignore Coches and others)
                        foreach (var player in completeTeam.Squad.Where(x => x.Role == "PLAYER"))
                        {
                            // Check if this player was already added in another team (like NATIONAL Selections) 
                            if (!await _repo.CheckIfEntityExistsByEntityId<Player>(x => x.Id == player.Id))
                            {
                                //Map the players and Add them
                                var newPlayer = _mapper.Map<Player>(player);
                                await _repo.Add(newPlayer);
                            }
                            
                            // Link the current player with the current Team
                            var teamPlayer = new TeamPlayer()
                            {
                                TeamId = team.Id,
                                PlayerId = player.Id,
                            };
                            await _repo.Add(teamPlayer);
                        }
                    }

                    // Link this team with the current competition
                    var teamComptetion = new CompetitionTeam()
                    {
                        CompetitionId = newCompetition.Id,
                        TeamId = team.Id,
                    };
                    await _repo.Add(teamComptetion);
                }
                // If everything was OK => 201
                return StatusCode(201, new { message = "Successfully imported" });
            }
            catch (System.Exception)
            {

                return StatusCode(504, new { message = "Server Error" });
            }

        }

        [HttpGet("total-players/{leagueCode}")]
        public async Task<IActionResult> TotalPlayers(string leagueCode) {
            //transform legueCode to Upper Case
            leagueCode = leagueCode.ToUpper();

            int totalPlayers = await _repo.TotalPlayers(leagueCode);
            return Ok(new {total = totalPlayers});
        }
    }
}