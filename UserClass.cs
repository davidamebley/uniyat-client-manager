﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjUniyatProject1
{
    static class UserClass
    {
        public static string id = "";
        public static string username = "", email = "", user_type = "";
        public static bool isUserTableChanged = false;

        public static void ResetCredentials()
        {
            username = "";
            email = "";
            user_type = "";
            id = "";
            isUserTableChanged = false;
        }
    }
}
