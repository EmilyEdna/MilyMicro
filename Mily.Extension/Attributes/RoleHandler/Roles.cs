using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Attributes.RoleHandler
{
    public static class Roles
    {
        public const String Admin = "Admin";
        public const String AdminCreate = "Admin.Create";
        public const String AdminRead = "Admin.Read";
        public const String AdminUpdate = "Admin.Update";
        public const String AdminDelete = "Admin.Delete";

        public const String User = "User";
        public const String UserCreate = "User.Create";
        public const String UserRead = "User.Read";
        public const String UserUpdate = "User.Update";
        public const String UserDelete = "User.Delete";
    }
}
