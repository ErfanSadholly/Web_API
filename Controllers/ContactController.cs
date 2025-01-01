using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using Web_Api.DTOs;
using Web_Api.PhoneBookRequest;
using Web_Api.Services;
using static Web_Api.Enums.SortEnums;



namespace Web_Api.Controllers
{

	[Route("api/[controller]")]
	[ApiController]

	public class ContactsController : ControllerBase
	{
		private readonly PhoneBookService _phoneBookService;
		private readonly ClaimService _claimService;


		public ContactsController(PhoneBookService phoneBookService, ClaimService claimService)
		{
			_phoneBookService = phoneBookService;
			_claimService = claimService;
		}

		// GET: api/Contacts
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] PhoneBookRequestParameters phoneBookRequest)
		{
			try
			{
				var response = await _phoneBookService.GetAllAsync(phoneBookRequest);

				if (!response.IsSuccess)
				{
					return NotFound(".هیچ مخاطبی یافت نشد");
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
		public async Task<IActionResult> Update(int id, [FromBody] PhoneBookWriteDTO PhoneBookDTO)
		{
			try
			{
				var userId = _claimService.GetUserIdFromClaims(User); // استخراج userId از Claims

				var response = await _phoneBookService.UpdateAsync(id, PhoneBookDTO, userId);
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

		// POST: api/Contacts
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] PhoneBookWriteDTO PhoneBookDTO)
		{
			try
			{
				var userId = _claimService.GetUserIdFromClaims(User);

				var response = await _phoneBookService.CreateAsync(PhoneBookDTO, userId);
				if(!response.IsSuccess) 
				{
					return BadRequest(response.Data.ID);
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
				var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
				if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
				{
					return Unauthorized("کاربر شناسایی نشد یا اطلاعات معتبر نیست.");
				}

				var response = await _phoneBookService.DeleteAsync(id, userId);
				if (!response.IsSuccess) 
				{
					return NotFound(response.Message);
				}

				return Ok(response.Data.ID);
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