using Microsoft.EntityFrameworkCore;
using work_01_MasterDetails.Models;
using work_01_MasterDetails.Repository.Interface;

namespace work_01_MasterDetails.Repository.Implementation
{
    public class SkillRepository : ISkillRepository
    {
        private readonly CandidateDbContext _context;

        public SkillRepository(CandidateDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skills>> GetAllSkillsAsync()
        {
            return await _context.Skills.ToListAsync();
        }
        public IEnumerable<Skills> Skills
        {
            get { return _context.Skills; }
        }
    }
}
