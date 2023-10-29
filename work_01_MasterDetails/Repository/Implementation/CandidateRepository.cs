using Microsoft.EntityFrameworkCore;
using work_01_MasterDetails.Models;
using work_01_MasterDetails.Models.ViewModels;
using work_01_MasterDetails.Repository.Interface;

namespace work_01_MasterDetails.Repository.Implementation
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CandidateDbContext _context;
        private readonly IWebHostEnvironment _he;

        public CandidateRepository(CandidateDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }

        public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
        {
            return await _context.Candidates.Include(x => x.CandidateSkills).ThenInclude(y => y.Skills).ToListAsync();
        }

        public async Task<Candidate> GetCandidateByIdAsync(int id)
        {
            return await _context.Candidates
                .Include(x => x.CandidateSkills)
                .ThenInclude(y => y.Skills)
                .FirstOrDefaultAsync(x => x.CandidateId == id);
        }

        public async Task<List<int>> GetSkillsByCandidateIdAsync(int candidateId)
        {
            return await _context.CandidateSkills
                .Where(cs => cs.CandidateId == candidateId)
                .Select(cs => cs.SkillsId)
                .ToListAsync();
        }

        public async Task AddCandidateAsync(CandidateVM candidateVM, int[] SkillsId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var candidate = new Candidate
                    {
                        CandidateName = candidateVM.CandidateName,
                        DateOfBirth = candidateVM.DateOfBirth,
                        Phone = candidateVM.Phone,
                        Fresher = candidateVM.Fresher
                    };

                    var file = candidateVM.ImagePath;
                    if (file != null)
                    {
                        string webroot = _he.WebRootPath;
                        string folder = "Images";
                        string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
                        string fileToSave = Path.Combine(webroot, folder, imgFileName);

                        using (var stream = new FileStream(fileToSave, FileMode.Create))
                        {
                            candidateVM.ImagePath.CopyTo(stream);
                            candidate.Image = "/" + folder + "/" + imgFileName;
                        }
                    }

                    _context.Candidates.Add(candidate);

                    foreach (var item in SkillsId)
                    {
                        CandidateSkills candidateSkills = new CandidateSkills()
                        {
                            Candidate = candidate,
                            CandidateId = candidate.CandidateId,
                            SkillsId = item
                        };
                        _context.CandidateSkills.Add(candidateSkills);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // Handle the exception as needed.
                }
            }
        }

        //public async Task UpdateCandidateAsync(CandidateVM candidateVM, int[] SkillsId)
        //{
        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var candidate = new Candidate
        //            {
        //                CandidateId = candidateVM.CandidateId,
        //                CandidateName = candidateVM.CandidateName,
        //                DateOfBirth = candidateVM.DateOfBirth,
        //                Phone = candidateVM.Phone,
        //                Fresher = candidateVM.Fresher,
        //                Image = candidateVM.Image
        //            };

        //            var file = candidateVM.ImagePath;
        //            if (file != null)
        //            {
        //                string webroot = _he.WebRootPath;
        //                string folder = "Images";
        //                string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
        //                string fileToSave = Path.Combine(webroot, folder, imgFileName);

        //                using (var stream = new FileStream(fileToSave, FileMode.Create))
        //                {
        //                    candidateVM.ImagePath.CopyTo(stream);
        //                    candidate.Image = "/" + folder + "/" + imgFileName;
        //                }
        //            }

        //            _context.Update(candidate);

        //            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
        //            foreach (var item in existSkill)
        //            {
        //                _context.CandidateSkills.Remove(item);
        //            }

        //            foreach (var item in SkillsId)
        //            {
        //                CandidateSkills candidateSkills = new CandidateSkills()
        //                {
        //                    CandidateId = candidate.CandidateId,
        //                    SkillsId = item
        //                };
        //                _context.CandidateSkills.Add(candidateSkills);
        //            }

        //            await _context.SaveChangesAsync();
        //            transaction.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Rollback();
        //            throw; // Handle the exception as needed.
        //        }
        //    }
        //}

        //public async Task UpdateCandidateAsync(CandidateVM candidateVM, int[] SkillsId)
        //{
        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var candidate = new Candidate
        //            {
        //                CandidateId = candidateVM.CandidateId,
        //                CandidateName = candidateVM.CandidateName,
        //                DateOfBirth = candidateVM.DateOfBirth,
        //                Phone = candidateVM.Phone,
        //                Fresher = candidateVM.Fresher
        //            };

        //            // Check if a new image file is provided
        //            if (candidateVM.ImagePath != null)
        //            {
        //                string webroot = _he.WebRootPath;
        //                string folder = "Images";
        //                string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
        //                string fileToSave = Path.Combine(webroot, folder, imgFileName);

        //                using (var stream = new FileStream(fileToSave, FileMode.Create))
        //                {
        //                    candidateVM.ImagePath.CopyTo(stream);
        //                    candidate.Image = "/" + folder + "/" + imgFileName;
        //                }
        //            }
        //            else
        //            {
        //                // No new image file provided, retain the existing image if any
        //                candidate.Image = candidateVM.Image;
        //            }

        //            _context.Update(candidate);

        //            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
        //            foreach (var item in existSkill)
        //            {
        //                _context.CandidateSkills.Remove(item);
        //            }

        //            foreach (var item in SkillsId)
        //            {
        //                CandidateSkills candidateSkills = new CandidateSkills()
        //                {
        //                    CandidateId = candidate.CandidateId,
        //                    SkillsId = item
        //                };
        //                _context.CandidateSkills.Add(candidateSkills);
        //            }

        //            await _context.SaveChangesAsync();
        //            transaction.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Rollback();
        //            throw; // Handle the exception as needed.
        //        }
        //    }
        //}

        public async Task UpdateCandidateAsync(CandidateVM candidateVM, int[] SkillsId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var candidate = new Candidate
                    {
                        CandidateId = candidateVM.CandidateId,
                        CandidateName = candidateVM.CandidateName,
                        DateOfBirth = candidateVM.DateOfBirth,
                        Phone = candidateVM.Phone,
                        Fresher = candidateVM.Fresher,
                    };

                    // Check if a new image is provided
                    if (candidateVM.ImagePath != null)
                    {
                        string webroot = _he.WebRootPath;
                        string folder = "Images";
                        string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
                        string fileToSave = Path.Combine(webroot, folder, imgFileName);

                        using (var stream = new FileStream(fileToSave, FileMode.Create))
                        {
                            candidateVM.ImagePath.CopyTo(stream);
                            candidate.Image = "/" + folder + "/" + imgFileName;
                        }
                    }
                    else
                    {
                        // No new image provided, keep the old image
                        candidate.Image = candidateVM.Image;
                    }

                    _context.Update(candidate);

                    // Remove existing skills
                    var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
                    foreach (var item in existSkill)
                    {
                        _context.CandidateSkills.Remove(item);
                    }

                    // Add new skills
                    foreach (var item in SkillsId)
                    {
                        CandidateSkills candidateSkills = new CandidateSkills()
                        {
                            CandidateId = candidate.CandidateId,
                            SkillsId = item
                        };
                        _context.CandidateSkills.Add(candidateSkills);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // Handle the exception as needed.
                }
            }
        }



        public async Task DeleteCandidateAsync(int id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
            if (candidate != null)
            {
                _context.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }
    }

}
