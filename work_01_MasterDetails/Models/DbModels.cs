using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace work_01_MasterDetails.Models
{
    public class Skills
    {
        public Skills()
        {
            this.CandidateSkills=new List<CandidateSkills>();
        }
        public int SkillsId { get; set; }
        public string SkillsName { get; set; }
        public virtual ICollection<CandidateSkills> CandidateSkills { get; set; }
    }
    public class Candidate
    {
        public Candidate()
        {
            this.CandidateSkills = new List<CandidateSkills>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        [DataType(DataType.Date),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool Fresher { get; set; }

        //nev
        public virtual ICollection<CandidateSkills> CandidateSkills { get; set; }
    }
    public class CandidateSkills
    {
        public int CandidateSkillsId { get; set; }
        [ForeignKey("Skills")]
        public int SkillsId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        //nev
        public virtual Skills Skills { get; set; }
        public virtual Candidate Candidate { get; set; }
    }

    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext(DbContextOptions<CandidateDbContext> options):base(options)
        {

        }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkills> CandidateSkills { get; set; }
    }
}
