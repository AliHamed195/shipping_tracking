using Microsoft.EntityFrameworkCore;

namespace shipping_tracking.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

    }
}
