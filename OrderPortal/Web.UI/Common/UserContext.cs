using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common
{
    public class UserContext
    {
        public UserContext()
        {
            //TODO: delete this statement once ClaimManager set this value correctly
            AccessorGuid = "FAB91841-8485-4A9A-8853-55CBFE72F119";
            IsReadonly = true;
            Organizations = new List<ClaimOrganization>();
        }

        public static UserContext Current
        {
            get
            {
                if (HttpContext.Current.Session[UIConstants.SessionUserContext] == null)
                {
                    HttpContext.Current.Session[UIConstants.SessionUserContext] = new UserContext();
                }
                return HttpContext.Current.Session[UIConstants.SessionUserContext] as UserContext;
            }
        }
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// List of organizations tagged to user.
        /// </summary>
        public IList<ClaimOrganization> Organizations { get; set; }

        /// <summary>
        /// Gets and Sets selected partner PCN number.
        /// </summary>
        public string PartnerPCN { get; set; }
        
        public string GetOrganizationName
        {
            get
            {
                var ret = Organizations.SingleOrDefault(c => c.PCN == PartnerPCN);
                return ret != null ? ret.OrganizationName : null;
            }
        }

        public bool IsRocUser
        {
            get { return Organizations.Count == 0; }
        }

        public bool CanSubmit { get; set; }
        public bool IsReadonly { get; set; }

        public string AccessorGuid { get; set; }

        public bool SetPcn(string pcn)
        {
            if (IsRocUser || Organizations.Any((c => c.PCN == pcn)))
            {
                PartnerPCN = pcn;
                return true;
            }
            return false;
        }
    }
}