using work_01_MasterDetails.Models;
using work_01_MasterDetails.Models.ViewModels;

namespace work_01_MasterDetails.Repository.Interface
{
    public interface ICandidateRepository
    {
        Task<IEnumerable<Candidate>> GetAllCandidatesAsync();
        Task<Candidate> GetCandidateByIdAsync(int id);
        Task<List<int>> GetSkillsByCandidateIdAsync(int candidateId);
        Task AddCandidateAsync(CandidateVM candidateVM, int[] SkillsId);
        Task UpdateCandidateAsync(CandidateVM candidateVM, int[] SkillsId);
        Task DeleteCandidateAsync(int id);
    }

}
