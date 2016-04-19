using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Web.UI.Common
{
    public class MyPrincipal : ClaimsPrincipal
    {

        public MyPrincipal(ClaimsPrincipal principal)
            : base(principal)
        { }

        public static new MyPrincipal Current
        {
            get
            {
                return new MyPrincipal(ClaimsPrincipal.Current);
            }
        }

        private string userName;
        /// <summary>
        /// Gets the account name of the user
        /// for the windows authenticated user, return user name in the format of DOMAIN\alias
        /// for the live id user, return the first name of the ID registered
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(userName))
                {
                    if (Claims != null)
                    {
                        if (Claims.All(c => c.Type != Constants.ClaimWindows) && Claims.Count(c=>c.Type == Constants.WindowsLiveName) == 1)
                        {
                            userName = Claims.Single(c => c.Type == Constants.WindowsLiveName).Value;
                        }
                        else
                        {
                            // Retrive the Windows Account Name in the format of DOMAIN\alias
                            if (Claims.Count(c => c.Type == Constants.ClaimWindows) == 1)
                            {
                                userName = Claims.Single(c => c.Type == Constants.ClaimWindows).Value;
                            }
                            else
                            {
                                if (Claims.Count(c => c.Type == Constants.ClaimFirstName) == 1)
                                {
                                    userName = Claims.Single(c => c.Type == Constants.ClaimFirstName).Value;
                                }
                                else
                                {
                                    userName = string.Empty;
                                }
                            }
                        }
                    }
                }
                return userName;
            }
            private set { userName = value; }
        }

        private string userAlias;

        /// <summary>
        /// Gets the user alias, for live id which does not have alias, return the first name
        /// </summary>
        public string UserAlias
        {
            get
            {
                if (string.IsNullOrEmpty(userAlias))
                {
                    if (Claims != null)
                    {
                        if (Claims.Any(c => c.Type == Constants.ClaimAlias))
                        {
                            userAlias = Claims.First(c => c.Type == Constants.ClaimAlias).Value;
                        }
                        else if (Claims.Any(c => c.Type == Constants.ClaimFirstName))
                        {
                            userAlias = Claims.First(c => c.Type == Constants.ClaimFirstName).Value;
                        }
                        else
                        {
                            userAlias = string.Empty;
                        }
                    }
                }
                return userAlias;
            }
            private set { userAlias = value; }
        }

        private string displayUserName;
        /// <summary>
        /// Return the name which will be displayed in the UI
        /// for the windows authenticated user, return the email address
        /// for the live id user, return the first name
        /// </summary>
        public string DisplayUserName
        {
            get
            {
                if (string.IsNullOrEmpty(displayUserName))
                {
                    if (Claims != null)
                    {
                        if (Claims.All(c => c.Type != Constants.ClaimWindows) && Claims.Count(c => c.Type == Constants.WindowsLiveName) == 1)
                        {
                            displayUserName = Claims.Single(c => c.Type == Constants.WindowsLiveName).Value;
                        }
                        else
                        {
                            if (Claims.Count(c => c.Type == Constants.ClaimEmail) == 1)
                            {
                                displayUserName = Claims.Single(c => c.Type == Constants.ClaimEmail).Value;
                            }
                            else
                            {
                                if (Claims.Count(c => c.Type == Constants.ClaimFirstName) == 1)
                                {
                                    displayUserName = Claims.Single(c => c.Type == Constants.ClaimFirstName).Value;
                                }
                                else
                                {
                                    displayUserName = string.Empty;
                                }
                            }
                        }
                    }
                }
                return userName;
            }
            set { displayUserName = value; }
        }

        /// <summary>
        /// Returns true if user credentials are authorized, else false.
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return Current.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// List of organizations tagged to user.
        /// </summary>
        public List<ClaimOrganization> Organizations { get; set; }

        /// <summary>
        /// Gets and Sets selected partner PCN number.
        /// </summary>
        public string PartnerPCN { get; set; }

        /// <summary>
        /// Gets assigned roles for the user.
        /// </summary>
        public string GetRoles
        {
            get
            {
                var claim=Claims.Single(c => c.Type == ClaimTypes.Role);
                return claim == null ? string.Empty : claim.Value;
            }
        }
    }

}
