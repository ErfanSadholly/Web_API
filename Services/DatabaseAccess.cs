﻿using Microsoft.Data.SqlClient;
using Web_Api.Interfaces;

namespace Web_Api.Services
{
	public class DatabaseAccess : IDatabaseAccess
	{
		private readonly string _connectionString;

		public DatabaseAccess(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
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
							return (firstName+" - "+lastName);
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
	}
}