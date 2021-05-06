using Microsoft.AspNetCore.Authorization;

namespace MartianRobot.Api.Policies
{
    public static class AdministratorPolicy
    {
        public const string Name = "AdministratorPolicy";

        public static void Build(AuthorizationPolicyBuilder builder)
            => builder.RequireRole("Administrator");
    }
}
