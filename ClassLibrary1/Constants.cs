using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Domain
{
    public class Constants
    {
        public static string GeneralManagerRole = "GeneralManager";

        public static string DepartmentManagerRole = "DepartmentManager";

        public static string TeamLeaderRole = "TeamLeader";

        public static string DeveloperRole = "Developer";

        public static string QARole = "QA";

        public static string OfficeManagerRole = "OfficeManager";

        public static int OfficeManagerAccessLevel = 255;

        public static int GeneralManagerAccessLevel = 1;

        public static int DepartmentManagerAccessLevel = 2;

        public static int TeamLeaderAccessLevel = 3;

        public static int DeveloperAccessLevel = 4;

        public static int QAAccessLevel = 4;
    }
}
