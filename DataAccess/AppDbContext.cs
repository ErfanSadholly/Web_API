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
				entity.Property(p => p.CreatedOn)
				.IsRequired()
				.HasColumnType("datetime");

				entity.Property(p => p.CreatedBy)
				.IsRequired()
				.HasColumnType("int");

				entity.Property(p => p.ModifiedOn)
				.IsRequired(false)
				.HasColumnType("datetime");

				entity.Property(p => p.ModifiedBy)
				.IsRequired(false)
				.HasColumnType("int");

				entity.Property(p => p.FirstName)
				.HasColumnType("nvarchar(100)");

				entity.Property(p => p.LastName)
				.HasColumnType("nvarchar(100)");

				entity.Property(p => p.PhoneNumber)
				.HasColumnType("varchar(50)");
			});

			builder.Entity<User>(entity =>
			{
				entity.Property(u => u.FirstName)
				.HasColumnType("nvarchar(100)")
				.IsRequired();

				entity.Property(u => u.LastName)
				.HasColumnType("nvarchar(100)")
				.IsRequired();
			});

		}
	}
}