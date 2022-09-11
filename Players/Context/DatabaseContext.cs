using Players.Models;
using System.Data.Entity;

namespace Players.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection") { }

        public DbSet<players> Players { get; set; }
        public DbSet<playerSkills> PlayerSkills { get; set; }
        
    }
}