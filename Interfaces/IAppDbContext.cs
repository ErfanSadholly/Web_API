using Microsoft.EntityFrameworkCore;
using Web_Api.Models.DbModels;

namespace Web_Api.Interfaces
{
	public interface IAppDbContext
	{
		DbSet<PhoneBook> PhoneBooks { get; } // دسترسی به PhoneBook
		DbSet<User> Users { get; } // دسترسی به User
		DbSet<Role> Roles { get; } // دسترسی به Role
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
