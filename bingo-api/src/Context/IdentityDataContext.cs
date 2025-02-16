using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Context;

public class IdentityDataContext : IdentityDbContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) { }
}
