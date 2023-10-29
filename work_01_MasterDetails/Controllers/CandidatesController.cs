using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using work_01_MasterDetails.Models;
using work_01_MasterDetails.Models.ViewModels;
using work_01_MasterDetails.Repository.Interface;

namespace work_01_MasterDetails.Controllers
{
    //public class CandidatesController : Controller
    //{
    //    private readonly CandidateDbContext _context;
    //    private readonly IWebHostEnvironment _he;
    //    public CandidatesController(CandidateDbContext context, IWebHostEnvironment he)
    //    {
    //        _context = context;
    //        _he = he;
    //    }
    //    public async Task<IActionResult> Index()
    //    {
    //        return View(await _context.Candidates.Include(x => x.CandidateSkills).ThenInclude(y => y.Skills).ToListAsync());
    //    }
    //    public IActionResult AddNewSkills(int? id)
    //    {
    //        ViewBag.skill = new SelectList(_context.Skills, "SkillsId", "SkillsName", id.ToString() ?? "");
    //        return PartialView("_AddNewSkills");
    //    }
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(CandidateVM candidateVM, int[] SkillsId)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            Candidate candidate = new Candidate
    //            {
    //                CandidateName = candidateVM.CandidateName,
    //                DateOfBirth = candidateVM.DateOfBirth,
    //                Phone = candidateVM.Phone,
    //                Fresher = candidateVM.Fresher
    //            };
    //            var file = candidateVM.ImagePath;
    //            string webroot = _he.WebRootPath;
    //            string folder = "Images";
    //            string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
    //            string fileToSave = Path.Combine(webroot, folder, imgFileName);
    //            if (file != null)
    //            {
    //                using (var stream = new FileStream(fileToSave, FileMode.Create))
    //                {
    //                    candidateVM.ImagePath.CopyTo(stream);
    //                    candidate.Image = "/" + folder + "/" + imgFileName;
    //                }
    //            }
    //            foreach (var item in SkillsId)
    //            {
    //                CandidateSkills candidateSkills = new CandidateSkills()
    //                {
    //                    Candidate = candidate,
    //                    CandidateId = candidate.CandidateId,
    //                    SkillsId = item
    //                };
    //                _context.CandidateSkills.Add(candidateSkills);
    //            }
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View();
    //    }
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
    //        CandidateVM candidateVM = new CandidateVM()
    //        {
    //            CandidateId = candidate.CandidateId,
    //            CandidateName = candidate.CandidateName,
    //            DateOfBirth = candidate.DateOfBirth,
    //            Phone = candidate.Phone,
    //            Image = candidate.Image,
    //            Fresher = candidate.Fresher
    //        };
    //        var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
    //        foreach (var item in existSkill)
    //        {
    //            candidateVM.SkillList.Add(item.SkillsId);
    //        }
    //        return View(candidateVM);
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(CandidateVM candidateVM, int[] SkillsId)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            Candidate candidate = new Candidate
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
    //            _context.Update(candidate);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View();
    //    }
    //    public async Task<IActionResult> Delete(int?id)
    //    {
    //        var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
    //        var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
    //        foreach (var item in existSkill)
    //        {
    //            _context.CandidateSkills.Remove(item);
    //        }
    //        _context.Remove(candidate);
    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }
    //}

    public class CandidatesController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ISkillRepository _skillRepository;

        public CandidatesController(ICandidateRepository candidateRepository, ISkillRepository skillRepository)
        {
            _candidateRepository = candidateRepository;
            _skillRepository = skillRepository;
        }

        public async Task<IActionResult> Index()
        {
            var candidates = await _candidateRepository.GetAllCandidatesAsync();
            return View(candidates);
        }

        public IActionResult AddNewSkills(int? id)
        {
            ViewBag.skill = new SelectList(_skillRepository.Skills, "SkillsId", "SkillsName", id.ToString() ?? "");
            return PartialView("_AddNewSkills");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateVM candidateVM, int[] SkillsId)
        {
            if (ModelState.IsValid)
            {
                await _candidateRepository.AddCandidateAsync(candidateVM, SkillsId);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var candidate = await _candidateRepository.GetCandidateByIdAsync((int)id);
            if (candidate == null)
            {
                return NotFound();
            }

            var candidateVM = new CandidateVM
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                DateOfBirth = candidate.DateOfBirth,
                Phone = candidate.Phone,
                Image = candidate.Image,
                Fresher = candidate.Fresher
            };

            // Use the repository method to get skills by candidate ID
            candidateVM.SkillList = await _candidateRepository.GetSkillsByCandidateIdAsync(candidate.CandidateId);

            return View(candidateVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CandidateVM candidateVM, int[] SkillsId)
        {
            if (ModelState.IsValid)
            {
                await _candidateRepository.UpdateCandidateAsync(candidateVM, SkillsId);
                return RedirectToAction(nameof(Index));
            }
            return View(candidateVM);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            await _candidateRepository.DeleteCandidateAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
    }

}
