using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;

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

			builder.Entity<PhoneBook>(entity =>
			{
				entity.Property(e => e.CreatedOn).IsRequired().HasColumnType("datetime");

				entity.Property(e => e.CreatedBy).IsRequired().HasColumnType("int");

				entity.Property(e => e.ModifiedOn).IsRequired(false).HasColumnType("datetime");

				entity.Property(e => e.ModifiedBy).IsRequired().HasColumnType("int");

				entity.Property(e => e.FirstName).HasColumnType("nvarchar(100)");

				entity.Property(e => e.LastName).HasColumnType("nvarchar(100)");

				entity.Property(e => e.PhoneNumber).HasColumnType("varchar(50)");
			});
		}
	}
}