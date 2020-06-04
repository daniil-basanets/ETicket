﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ETicketMobile.WebAccess.Network.Endpoints
{
    public class UsersEndpoint
    {
        public static Uri GetUserByEmail = new Uri("/api/User"); //create an endpoint at the server

        public static Uri GetDocumentById = new Uri("/api/GetDocumentById");

        public static Uri GetPrivilegeById = new Uri("/api/GetPrivilegeById");
    }
}
