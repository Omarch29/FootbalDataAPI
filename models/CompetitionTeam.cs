namespace FootbalDataAPI.models
{
    public class CompetitionTeam
    {
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}