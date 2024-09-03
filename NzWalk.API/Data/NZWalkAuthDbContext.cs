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

        protected override void OnModelCreating(ModelBuilder builder)
            {
            base.OnModelCreating(builder);
            var readerRoleId = "0c7d8da2-a610-4b56-becb-6c205078a437";
            var writerRoleId = "28c9d9da-00ee-4b14-83be-2a27a6dc580f";

            var roles = new List<IdentityRole>
                {
                new IdentityRole()
                    {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId ,
                    Name = "Reader" ,
                    NormalizedName = "Reader".ToUpper() ,
                    }  ,
                
                new IdentityRole()
                    {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId ,
                    Name = "Writer" ,
                    NormalizedName = "Writer".ToUpper() ,
                    }
                };
            builder.Entity<IdentityRole>().HasData(roles);
            }
        }
    }
