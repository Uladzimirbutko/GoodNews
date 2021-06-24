using Microsoft.AspNetCore.Authorization;

namespace NewsAggregator.AuthorizationPolicies
{
    public class MinAgeRequirement : IAuthorizationRequirement
    {
        public int MinAge { get; }

        public MinAgeRequirement(int minAge)
        {
            MinAge = minAge;
        }

    }
}