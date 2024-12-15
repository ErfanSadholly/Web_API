using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Web_Api.AppData;
using Web_Api.DbModels;
using Web_Api.DTOs;
using Web_Api.Interfaces;
using Web_Api.Services;
using WebApi.Repositories;


namespace Web_Api.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]

	public class ContactsController : ControllerBase
	{
		private readonly PhoneBookService _phoneBookService;

		public ContactsController(PhoneBookService phoneBookService)
		{
			_phoneBookService = phoneBookService;
		}

		// GET: api/Contacts
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var response = await _phoneBookService.GetAllAsync();
				if (!response.IsSuccess) 
				{
					return NotFound(response.Message);
				}

				return Ok(response.Data);
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET: api/Contacts/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var response = await _phoneBookService.GetByIdAsync(id);
				if (!response.IsSuccess) 
				{
					return NotFound(response.Message);
				}

				return Ok(response.Data);
			}
			catch (KeyNotFoundException ex)
			{ 
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// PUT: api/Contacts/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] PhoneBookDTO PhoneBookDTO)
		{
			try
			{
				var response = await _phoneBookService.UpdateAsync(id, PhoneBookDTO);
				if (!response.IsSuccess) 
				{
					return NotFound(response.Message);
				}

				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// POST: api/Contacts
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] PhoneBookDTO PhoneBookDTO)
		{
			try
			{
				var response = await _phoneBookService.CreateAsync(PhoneBookDTO);
				if(!response.IsSuccess) 
				{
					return BadRequest(response.Message);
				}

				return CreatedAtAction("GetById", new { id = response.Data.ID }, response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// DELETE: api/Contacts/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var response = await _phoneBookService.DeleteAsync(id);
				if (!response.IsSuccess) 
				{
					return NotFound(response.Message);
				}

				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			} 
		}
	}
}

