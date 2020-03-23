using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.Owin;

namespace Gallery.PL.Interfaces
{
    public interface IAuthenticationService
    {
        void AuthByOwinCookies(IOwinContext owinCtx, ClaimsIdentity claim);

        ClaimsIdentity ClaimsСreation(string claims);

        void LogOut(IOwinContext owinCtx);
    }
}