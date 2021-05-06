using System;
using System.Security.Claims;

namespace MartianRobot.Api.Extensions
{
    public static class ClaimConstants
    {
        public const string Name = "name";
        public const string ObjectId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string Oid = "oid";
        public const string PreferredUserName = "preferred_username";
        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";
        public const string Tid = "tid";
        public const string Scope = "http://schemas.microsoft.com/identity/claims/scope";
        public const string Roles = "roles";
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var oid = GetObjectId(claimsPrincipal);
            if (oid == null)
            {
                throw new InvalidProgramException("Invalid user claims");
            }

            return oid.ToString();
        }

        private static object GetObjectId(this ClaimsPrincipal claimsPrincipal)
        {
            string userObjectId = claimsPrincipal.FindFirstValue(ClaimConstants.Oid);
            if (string.IsNullOrEmpty(userObjectId))
                userObjectId = claimsPrincipal.FindFirstValue(ClaimConstants.ObjectId);

            return userObjectId;
        }

        public static string GetDisplayName(this ClaimsPrincipal claimsPrincipal)
        {
            // Attempting the claims brought by an Microsoft identity platform token first
            string displayName = claimsPrincipal.FindFirstValue(ClaimConstants.PreferredUserName);

            // Otherwise falling back to the claims brought by an Azure AD v1.0 token
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = claimsPrincipal.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            }

            // Finally falling back to name
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = claimsPrincipal.FindFirstValue(ClaimConstants.Name);
            }

            return displayName;
        }
    }
}
