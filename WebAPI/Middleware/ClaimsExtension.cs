using System.Security.Claims;
using System;
using Entities.Enums;
using System.Drawing;
using System.Reflection;

namespace WebAPI.Middleware
{
    public static class ClaimsExtension
    {
        public static T GetLoggedInUserId<T>(this ClaimsPrincipal principal)
        {
            var loggedInUserId = principal.FindFirstValue("id");
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(loggedInUserId, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return loggedInUserId != null ? (T)Convert.ChangeType(loggedInUserId, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
            }
            else
            {
                throw new Exception("Invalid type provided");
            }
        }

        public static string GetLoggedInUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue("UserName");
        }
        public static string GetLoggedInUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue("UserName");
        }

        public static string GetLoggedInUserRoles(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Role);

        }

        public static T GetLoggedInUserRole<T>(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            string value = principal.FindFirstValue(ClaimTypes.Role);
            if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                if (Enum.TryParse<Role>(value, out Role result))
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                throw new Exception("Invalid type provided");
            }
        }

        public static string GetLoggedInUserRoleId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.Role);

        }

        public static object GetLoggedInUserRolesList(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var roles = principal.FindAll(ClaimTypes.Role);
            return roles;
        }
    }
}
