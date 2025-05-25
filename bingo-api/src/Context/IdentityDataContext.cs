using bingo_api.src.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Context;

public class IdentityDataContext : IdentityDbContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) { }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
                .HasIndex(u => u.EntityId)
                .IsUnique();  // 👈 Isso garante que `EntityId` seja único
        base.OnModelCreating(modelBuilder);


    }
}
