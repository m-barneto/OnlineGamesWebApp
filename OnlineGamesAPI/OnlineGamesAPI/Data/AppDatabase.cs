using Microsoft.EntityFrameworkCore;
using OnlineGamesAPI.Data.Models;
using System.Reflection;

namespace OnlineGamesAPI.Data {
    public class AppDatabase : DbContext {

        public AppDatabase(DbContextOptions<AppDatabase> options) : base(options) { }

        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<InviteModel> Invites => Set<InviteModel>();
        public DbSet<FillerGameModel> FillerGames => Set<FillerGameModel>();

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlite(options => {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<InviteModel>().ToTable("Invites");
            modelBuilder.Entity<FillerGameModel>().ToTable("Games");



            /*
            modelBuilder.Entity<InviteModel>().ToTable("Invites", "test");

            List<UserModel> users = new List<UserModel>();
            for (int i = 0; i < 10; i++) {
                users.Add(new UserModel { Id = $"test{i}", Email = $"email{i}@gmail.com", AccountCreateTime = new DateTime(2021, i + 1, 20).Ticks, LastSigninTime = DateTime.Now.Ticks });
            }
            modelBuilder.Entity<UserModel>().HasData(users);

            List<InviteModel> invites = new List<InviteModel>();
            for (int i = 0; i < 10; i++) {
                invites.Add(new InviteModel { CreatorId = $"test{i}", InviteCreationTime = new DateTime(2021, i + 1, 20).Ticks, InviteData = "blob"});
            }
            modelBuilder.Entity<InviteModel>().HasData(invites);
             */

            base.OnModelCreating(modelBuilder);
        }
    }
}
