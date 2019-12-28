using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FootbalDataAPI.DATA
{
    public interface IFootballDataRepository
    {
        Task<T> Add<T>(T entity) where T : class;
        Task<bool> CompetitionExists(int competitionId);
        Task<int> TotalPlayers (string leagueCode);
        Task<bool> CheckIfEntityExistsByEntityId<T>(Expression<Func<T, bool>> expr) where T : class;
    }
}