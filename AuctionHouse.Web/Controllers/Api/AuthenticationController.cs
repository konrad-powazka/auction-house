using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using AuctionHouse.Web.Models;
using Microsoft.AspNet.Identity;

namespace AuctionHouse.Web.Controllers.Api
{
	[AllowAnonymous]
	public class AuthenticationController : ApiController
    {
        [HttpPost]
        public void SignIn(LoginCommand command)
        {
            // For test purposes we authenticate any user name/password combination
            var userNameClaim = new Claim(ClaimTypes.Name, command.UserName);
            var userNameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, command.UserName);

            var claimsIdentity = new ClaimsIdentity(new[] {userNameClaim, userNameIdentifierClaim},
                DefaultAuthenticationTypes.ApplicationCookie);

            Request.GetOwinContext().Authentication.SignIn(claimsIdentity);
        }

        [HttpPost]
        public void SignOut()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

		[HttpGet]
		public User GetCurrentUser()
		{
			return User.Identity.IsAuthenticated ? new User(User.Identity.Name) : null; 
		}
	}
}