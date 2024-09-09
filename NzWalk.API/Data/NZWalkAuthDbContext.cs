using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace NzWalk.API.Data
    {
    public class NZWalkAuthDbContext : IdentityDbContext
        {
        public NZWalkAuthDbContext(DbContextOptions<NZWalkAuthDbContext> options) : base(options)
            {
            }

        // Seeding roles into the database during model creation
        protected override void OnModelCreating(ModelBuilder builder)
            {
            base.OnModelCreating(builder);

            // Predefined IDs for the roles
            var readerRoleId = "0c7d8da2-a610-4b56-becb-6c205078a437";
            var writerRoleId = "28c9d9da-00ee-4b14-83be-2a27a6dc580f";

            // Defining the "Reader" and "Writer" roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER"
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER"
                }
            };

            // Seeding the roles into the IdentityRole entity
            builder.Entity<IdentityRole>().HasData(roles);
            }
        }
    }
