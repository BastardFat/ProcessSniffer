using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminServer.Models;

namespace AdminServer.StaticConigs
{
    public static class Roles
    {
        public static List<Models.Role> AllRoles = GetRoles();

        private static List<Role> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}