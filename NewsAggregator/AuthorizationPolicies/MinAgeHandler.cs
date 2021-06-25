using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NewsAggregator.AuthorizationPolicies
{
    public class MinAgeHandler : AuthorizationHandler<MinAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinAgeRequirement requirement)
        {
            if (context.User.HasClaim(cl => cl.Type == "age"))
            {
                var age = Convert.ToInt32(context.User
                    .FindFirst(cl => cl.Type == "age")
                    .Value);

                if (age >= requirement.MinAge && age <= 100)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}