using System.Collections.Generic;

namespace FootbalDataAPI.models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tla { get; set; }
        public string ShortName { get; set; }
        public string AreaName { get; set; }
        public string Email { get; set; }
        public IList<CompetitionTeam> Competitions { get; set; }
        public IList<Player> Squad { get; set; }
    }
}