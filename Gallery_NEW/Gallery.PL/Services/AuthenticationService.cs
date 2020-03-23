using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Gallery.PL.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Gallery.PL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        /*private readonly IOwinContext _owinCtx;

        AuthenticationService(IOwinContext owinCtx)
        {
            _owinCtx = owinCtx ?? throw new ArgumentNullException(nameof(owinCtx));
        }*/

        public void AuthByOwinCookies(IOwinContext owinCtx, ClaimsIdentity claim)
        {
            owinCtx.Authentication.SignOut();
            owinCtx.Authentication.SignIn(new AuthenticationProperties
            {
                IsPersistent = true
            }, claim);
        }

        public ClaimsIdentity ClaimsСreation(string claims)
        {
            ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, claims, ClaimValueTypes.String));
            return claim;
        }

        public void LogOut(IOwinContext owinCtx)
        {
            owinCtx.Authentication.SignOut();
        }
    }
}