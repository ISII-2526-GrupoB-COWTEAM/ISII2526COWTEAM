using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Scale> Scale { get; set; }
    public DbSet<Repair> Repair { get; set; }
    public DbSet<Receipt> Receipt { get; set; }
    public DbSet<ReceiptItem> ReceiptItem { get; set; }
}
