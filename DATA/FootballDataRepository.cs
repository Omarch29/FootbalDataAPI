using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Football.API.DATA;
using System.Linq.Expressions;
using System;

namespace FootbalDataAPI.DATA
{
    public class FootballDataRepository : IFootballDataRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FootballDataRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T> Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> CompetitionExists(int competitionId)
        {
            return await _context.Competitions.AnyAsync(c => c.Id == competitionId);
        }

        public async Task<bool> CheckIfEntityExistsByEntityId<T>(Expression<Func<T, bool>> expr) where T : class
        {
            DbSet<T> DbSet = _context.Set<T>();
            return await DbSet.AnyAsync(expr);
        }

        public async Task<int> TotalPlayers(string leagueCode)
        {
            var competition = await _context.Competitions.Where(c => c.Code == leagueCode).FirstOrDefaultAsync();

            var result = await (from pt in _context.TeamPlayers
                                join tm in _context.Teams on pt.TeamId equals tm.Id
                                where tm.Competitions.Any(t => t.CompetitionId == competition.Id)
                                select pt).CountAsync();
            return result;
        }
    }
}