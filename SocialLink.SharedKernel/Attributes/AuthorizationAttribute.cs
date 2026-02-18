using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialLink.SharedKernel.Domain;
using SocialLink.SharedKernel.Enumerators;

namespace SocialLink.SharedKernel.Attributes;

public class AuthorizationFilter(IIdentityUser currentUser, eSystemRole[] roles) : IAsyncAuthorizationFilter
{
	public Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		if (!currentUser.IsAuthenticated)
		{
			context.Result = new UnauthorizedResult();
			return Task.CompletedTask;
		}

		var hasAny = currentUser.Roles?.Any(_ => roles.Contains(_)) == true;

		if (!hasAny)
			context.Result = new ForbidResult();

		return Task.CompletedTask;
	}
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class AuthorizationAttribute : TypeFilterAttribute
{
	public AuthorizationAttribute(params eSystemRole[] roles)
		: base(typeof(AuthorizationFilter))
	{
		Arguments = new object[] { roles };
	}
}