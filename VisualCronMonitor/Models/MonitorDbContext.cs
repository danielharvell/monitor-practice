using System.Data.Entity;

namespace VisualCronMonitor.Models
{
    public class MonitorDbContext : DbContext
    {
        public MonitorDbContext() : base("name=EntityFrameworkConnectionString")
        {
            //Debug.WriteLine("{0}", Database.Connection.ConnectionString);
        }

        public DbSet<Task> Tasks { get; set; }
    }
}