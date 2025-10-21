using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Device> Device { get; set; }
    public DbSet<Model> Model { get; set; }
    public DbSet<ReviewItem> ReviewItem { get; set; }
    public DbSet<Review> Review { get; set; }

    
}
