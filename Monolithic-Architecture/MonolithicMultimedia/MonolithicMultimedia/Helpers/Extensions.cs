﻿using System.Linq;
using System.Security.Claims;

namespace MonolithicMultimedia.Helpers
{
    public static class Extensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var id = user.Claims
                .Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            return id;
        }
    }
}
