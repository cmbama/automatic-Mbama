using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using Microsoft.Owin.Security;
using BTDatabaseApplicationAppFramework;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Web.Hosting;

namespace BTDatabaseApplicationAppFramework.Models
{
    public class AdAuthenticationService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class AuthenticationResult
        {
            public AuthenticationResult(string errorMessage = null)
            {
                ErrorMessage = errorMessage;
            }

            public String ErrorMessage { get; private set; }
            public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        }

        private readonly IAuthenticationManager authenticationManager;

        public AdAuthenticationService(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }


        /// <summary>
        /// Check if username and password matches existing account in AD. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticationResult SignIn(String username, String password)
        {
#if DEBUG
     
            // authenticates against your local machine - for development time
            ContextType authenticationType = ContextType.Domain;

            //string UserName = null;
            string MachineName = null;
            //The MachineName property gets the name of your computer.
            MachineName = System.Environment.MachineName;

#else
            // authenticates against your Domain AD
            ContextType authenticationType = ContextType.Domain;
#endif


            //using(PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Machine, MachineName, null, ContextOptions.Negotiate, username, password))
            //using (GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincipalContext, Settings.AdministratorsGroup))
            //string usernamer = "DXCDC12HT4001.ead.csc.com\\Cajetan.Mbama@dxc.com";
            //PrincipalContext principalContext = new PrincipalContext(authenticationType, "10.167.32.140:389");
            PrincipalContext principalContext = new PrincipalContext(authenticationType);
            bool isAuthenticated = false;
            UserPrincipal userPrincipal = null;
            try
            {
                isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                if (isAuthenticated)
                {
                    userPrincipal = UserPrincipal.FindByIdentity(principalContext,  username);                  
                }
            }
            catch (Exception ex)
            {
                isAuthenticated = false;
                userPrincipal = null;
                logger.Error(ex.Message + " AuthenticationResult SignIn(String username, String password)");
               
            }

            if (!isAuthenticated || userPrincipal == null)
            {
                return new AuthenticationResult("Username or Password is not correct");
            }

            if (userPrincipal.IsAccountLockedOut())
            {
                // here can be a security related discussion weather it is worth 
                // revealing this information
                return new AuthenticationResult("Your account is locked.");
            }

            if (userPrincipal.Enabled.HasValue && userPrincipal.Enabled.Value == false)
            {
                // here can be a security related discussion weather it is worth 
                // revealing this information
                return new AuthenticationResult("Your account is disabled");
            }

            var identity = CreateIdentity(userPrincipal);

            authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);


            return new AuthenticationResult();
        }


        private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        {
            var identity = new ClaimsIdentity(MyAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.SamAccountName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));
            if (!String.IsNullOrEmpty(userPrincipal.EmailAddress))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));
                logger.Info("Current time: "+ DateTime.Now + " , Username/principal :" + userPrincipal.SamAccountName + " , UserEmail : " + userPrincipal.EmailAddress);
                
            }

            // add your own claims if you need to add more information stored on the cookie

            return identity;
        }
    }
}