using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Authorization
{
    public static class Constants
    {
        public static class PolicyNames
        {
            public const string HasNationality = "HasNationality";
            public const string AtLeast20 = "AtLeast20";
            public const string CraetedAtLeast2Restaurants = "CraetedAtLeast2Restaurants";


        }
        public static class AppClaimTypes
        {
            public const string Nationality = "HasNationality";
            public const string DateOfBirth = "DateOfBirth";

        }
    }
}

