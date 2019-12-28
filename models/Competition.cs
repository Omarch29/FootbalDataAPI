using System.Collections.Generic;

namespace FootbalDataAPI.models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AreaName { get; set; }

        public IList<CompetitionTeam> Teams { get; set; }
    }
}