﻿using System.Security.Claims;

namespace RentalsWebApp
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role).Value;
        }
        public static string GetMonth(this ClaimsPrincipal user)
        {
            return user.FindFirst(DateTime.Now.Month.ToString()).Value;
        }

    }
}
