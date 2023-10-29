using work_01_MasterDetails.Models;

namespace work_01_MasterDetails.Repository.Interface
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skills>> GetAllSkillsAsync();
        IEnumerable<Skills> Skills { get; }
    }
}
