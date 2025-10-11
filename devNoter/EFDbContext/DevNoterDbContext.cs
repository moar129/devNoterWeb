using devNoter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace devNoter.EFDbContext
{
    public class DevNoterDbContext : IdentityDbContext<ApplicationUser>
    {
        public DevNoterDbContext(DbContextOptions<DevNoterDbContext> options)
            : base(options)
        {
        }

        // Keep your custom entities
        public DbSet<Note> Notes { get; set; }
        public DbSet<LanguageFolder> LanguageFolders { get; set; }

        // <-- Add OnModelCreating here
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User → LanguageFolders
            builder.Entity<LanguageFolder>()
                   .HasOne(f => f.User)
                   .WithMany()
                   .HasForeignKey(f => f.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // ParentFolder → ChildFolders (self-reference)
            builder.Entity<LanguageFolder>()
                   .HasMany(f => f.ChildFolders)
                   .WithOne(f => f.ParentFolder)
                   .HasForeignKey(f => f.ParentFolderId)
                   .OnDelete(DeleteBehavior.Restrict); // <- prevent SQL Server multiple cascade paths error
        }
    }
}
