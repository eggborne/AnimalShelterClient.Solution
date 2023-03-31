using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AnimalShelterClient.Models
{
    public class AnimalShelterClientContext : IdentityDbContext<ApplicationUser>
    {
        public AnimalShelterClientContext(DbContextOptions options) : base(options) { }
    }
}