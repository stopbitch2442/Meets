using Microsoft.EntityFrameworkCore;
using Meets.WebApi.MeetupFolder;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Meets.WebApi
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<MeetupEntity> Meetups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseNpgsql("Server=localhost;Port=5432;Database=asp_courses;User Id=user;Password=password");

    }
}
