using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Web_Api.Interfaces;
using Web_Api.Models.DbModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Web_Api.Services
{
	public class DatabaseAccessService : IDatabaseAccess
	{
		private readonly string _connectionString;
		private readonly IAppDbContext _DbContext;

		public DatabaseAccessService(IConfiguration configuration, IAppDbContext appDbContext)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
			_DbContext = appDbContext;
		}

		public async Task<string> GetUserFullName(int UserId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				const string query = "SELECT FirstName, LastName FROM AspNetUsers WHERE ID = @UserId";
				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@UserId", UserId);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							string firstName = reader["FirstName"].ToString();
							string lastName = reader["LastName"].ToString();
							return (firstName + " - " + lastName);
						}
					}
				}
			}
			return string.Empty;
		}
		public async Task<bool> CanEditPhoneBook(int userId, int phoneBookId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				const string query = @"
                    SELECT CASE 
                        WHEN CreatedBy = @UserId THEN 1 
                        ELSE 0 
                    END AS CanEdit
                    FROM PhoneBooks
                    WHERE ID = @PhoneBookId";

				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@UserId", userId);
					command.Parameters.AddWithValue("@PhoneBookId", phoneBookId);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							// اگر نتیجه 1 باشد، یعنی کاربر خودش این مخاطب را ایجاد کرده است
							return reader.GetInt32(0) == 1;
						}
					}
				}
			}
			return false;
		}

		public async Task<string> GetUserFullNameWithEF(int UserId)
		{
			var query = @"SELECT
			    FirstName , LastName
				FROM
				AspNetUsers
				WHERE
				Id = @UserId";

			var user = await _DbContext
				.Users
				.FromSqlRaw(query, new SqlParameter("@UserId", UserId))
				.Select(u => new { u.FirstName, u.LastName })
				.FirstOrDefaultAsync();

			return user != null ? $"{user.FirstName} - {user.LastName}" : string.Empty;
		}

		public async Task<string> GetUserFullNameWithDapper(int UserId)
		{
			var query = @"Select
			FirstName , LastName
			from
			AspNetUsers
			Where
			Id = @UserId";

			using (var connection = new SqlConnection(_connectionString))
			{
				var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { UserId = UserId });

				return user != null ? $"{user.FirstName} - {user.LastName}" : string.Empty;
			}
		}
	}
}