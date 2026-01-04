using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Infrastructure.Data.Identity
{
    public class DbContextForIdentity:IdentityDbContext<AppUser>
    {
        public DbContextForIdentity(DbContextOptions<DbContextForIdentity> options) :
            base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
