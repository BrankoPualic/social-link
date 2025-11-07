using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialLink.SharedKernel;
using SocialLink.Users.Enumerators;

namespace SocialLink.Users.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[AllowAnonymous]
internal class UserMasterDataController : ControllerBase
{
	[HttpGet]
	public IActionResult GetGenders()
	{
		var result = Enum.GetValues<eGender>()
			.Select(LookupDto.EnumProjection)
			.ToList();

		return Ok(result);
	}
}