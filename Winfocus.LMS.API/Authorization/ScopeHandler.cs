using Microsoft.AspNetCore.Authorization;

namespace Winfocus.LMS.API.Authorization
{
    public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
      protected override Task HandleRequirementAsync(
       AuthorizationHandlerContext context,
       ScopeRequirement requirement)
        {
            var userCountryId = context.User.FindFirst("CountryId")?.Value;
            var userCenterId = context.User.FindFirst("CenterId")?.Value;

            if (context.Resource is HttpContext httpContext)
            {
                var routeCountryId = httpContext.Request.RouteValues["countryId"]?.ToString();
                var routeCenterId = httpContext.Request.RouteValues["centerId"]?.ToString();

                bool countryMatch = string.IsNullOrEmpty(routeCountryId) || routeCountryId == userCountryId;
                bool centerMatch = string.IsNullOrEmpty(routeCenterId) || routeCenterId == userCenterId;

                if (countryMatch && centerMatch)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
