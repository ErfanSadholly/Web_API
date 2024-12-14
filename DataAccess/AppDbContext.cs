using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Api.DbModels;
using Web_Api.Interfaces;

namespace Web_Api.AppData
{
	public class AppDbContext : IdentityDbContext<User, Role, int> , IAppDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<PhoneBook> PhoneBooks { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<PhoneBook>().HasQueryFilter(p => !p.Deleted);
		}
	}
}
