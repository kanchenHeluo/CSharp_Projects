using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading;
using Web.UI.Security;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using EBAC;

namespace Web.UI.Common
{
    public static class ClaimManager
    {
        public static void BuildClaim(string appGUID)
        {
            var userDetails = UserContext.Current;
            var requestUrl = HttpContext.Current.Request.Url.ToString();
            if (!userDetails.IsAuthorized && !requestUrl.Contains(Utility.GetUrlHelper().UnAuthorized()) && !requestUrl.Contains(Utility.GetUrlHelper().SignOut()))
            {
                var currentPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;

                var isNtAuth = currentPrincipal.Claims.Any(c => c.Type == Constants.ClaimWindows);
                string authenticationType;
                string authenticationValue;
                if (!isNtAuth)
                {
                    authenticationType = Constants.WindowsLiveAuthentication;
                    authenticationValue = currentPrincipal.Claims.First(c => c.Type == Constants.ClaimPartnerImmutableID).Value;
                }
                else
                {
                    authenticationType = Constants.NTAuthentication;
                    authenticationValue = currentPrincipal.Claims.First(c => c.Type == Constants.ClaimWindows).Value;
                }

                var tableClaims = SqlHelper.ExecProcSqlDataReader("GetALLClaims", new[]{new SqlParameter("CredentialType", authenticationType),new SqlParameter("CredentialValue", authenticationValue)}, Constants.AccessorDB);

                if (tableClaims == null)
                {
                    HttpContext.Current.Response.Redirect(Utility.GetUrlHelper().UnAuthorized());
                    return;
                }
                var claimsRows = tableClaims.AsEnumerable();
                var pcnNos = (from claimsRow in claimsRows where claimsRow.Field<string>("ClaimType").ToLower() == "organizationpcn" select (string)claimsRow["ClaimValue"]).ToArray<string>();
                var name = (from claimsRow in claimsRows where claimsRow.Field<string>("ClaimType").ToLower() == "name" select (string)claimsRow["ClaimValue"]).ToArray<string>();
                for (var i = 0; i < pcnNos.Length; i++)
                {
                    userDetails.Organizations.Add(new ClaimOrganization {PCN = pcnNos[i], OrganizationName = name[i]});
                }
                var contactPcn = (from claimsRow in claimsRows where claimsRow.Field<string>("ClaimType").ToLower() == "contactpcn" select (string)claimsRow["ClaimValue"]).FirstOrDefault();
                if (!string.IsNullOrEmpty(contactPcn))
                {
                    GetContactClaims(contactPcn, userDetails);
                    if (name.Length > 0)
                    {
                        var displayName = new Claim(Constants.ClaimNamespace + "Name", name[0], name[0].GetType().ToString(), "Accessor");
                        var id = currentPrincipal.Identities.First();
                        id.AddClaim(displayName);
                    }
                }

                var ebacValue = (from claimsRow in claimsRows where claimsRow.Field<string>("ClaimType").ToLower() == "ebac" select (string)claimsRow["ClaimValue"]).FirstOrDefault();
                if (!ParseEbac(ebacValue, userDetails))
                {
                    HttpContext.Current.Response.Redirect(Utility.GetUrlHelper().UnAuthorized(), true);
                    return;
                }
                var otherRoles = (from claimsRow in claimsRows where claimsRow.Field<string>("IsRole").ToLower() == "y" select claimsRow).ToList<DataRow>();
                //var vlcmRoles = (from claimsRow in claimsRows
                //                 where claimsRow.Field<string>("IsRole").ToLower() == "y" &&
                //                     claimsRow.Field<string>("ApplicationName").Contains("VLCM")
                //                 select claimsRow).ToList<DataRow>();
                BuildOtherRoles(otherRoles);
                if (userDetails.Organizations != null && userDetails.Organizations.Any())
                {
                    userDetails.SetPcn(userDetails.Organizations[0].PCN);
                }
                userDetails.IsAuthorized = true;

                HttpContext.Current.Session[UIConstants.SessionShowPartnerModal] = userDetails.Organizations != null && userDetails.Organizations.Count > 1;
            }
        }

        private static void BuildOtherRoles(List<DataRow> otherRoles)
        {
            var umRoles = otherRoles.Where(c => (c.Field<string>("ClaimValue").ToLower().Contains("usermanager") ||
                c.Field<string>("ClaimValue").ToLower().Contains("pum")));
            var emslRoles = otherRoles.Where(c => (c.Field<string>("ClaimValue").ToLower() != "usermanager" &&
                 c.Field<string>("ClaimValue").ToLower() != "pum"));
            if (emslRoles != null)
            {
                var emslRolesCollection = string.Join(",", emslRoles.Select(c => c.Field<string>("ClaimValue").ToLower()).ToArray<string>());
                AddRolestoClaims(emslRolesCollection, "eMSLRole");
            }
            if (umRoles != null)
            {
                var umRolesCollection = string.Join(",", umRoles.Select(c => c.Field<string>("ClaimValue").ToLower()).ToArray<string>());
                AddRolestoClaims(umRolesCollection, "UMRole");
            }
        }

        /// <summary>
        /// Adding user roles to Principal claims.
        /// </summary>
        /// <param name="roles">
        /// EBAC decrypted roles
        /// </param>
        private static void AddRolestoClaims(string roles, string roleName = "")
        {
            var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Claim claimRole;
            var id = claimsPrincipal.Identities.First();
            claimRole = !string.IsNullOrEmpty(roleName) ? new Claim(Constants.ClaimNamespace + roleName, roles, ClaimValueTypes.String, "Accessor") : new Claim(ClaimTypes.Role, roles);
            id.AddClaim(claimRole);
        }

        /// <summary>
        /// Adding claims to the Principal
        /// </summary>
        /// <param name="claimsList">
        /// Claimlist to be added to Principal.
        /// </param>
        private static void AddClaims(IEnumerable<Claim> claimsList)
        {
            var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var id = claimsPrincipal.Identities.First();
            foreach (var claim in claimsList)
            {
                id.AddClaim(claim);
            }
        }

        /// <summary>
        /// Get Claims for the user based on the Contact PCN.
        /// </summary>
        /// <param name="ContactPCNVal">
        /// Contact PCN String.
        /// </param>
        /// <param name="userDetails"></param>
        private static void GetContactClaims(string ContactPCNVal, UserContext userDetails)
        {
            ContactRoleClaim contactClaim = new ContactRoleClaim();
            contactClaim.ContactRoleTypeClaim = new ContactRoleClaimContactRoleTypeClaim();
            contactClaim.ContactRoleTypeClaim.ContactRoleTypeId = "22";
            SqlParameter contactGuid = new SqlParameter("ContactGuid", null);
            SqlParameter ContactPCN = new SqlParameter("ContactPCN", ContactPCNVal);
            SqlParameter ApplicationGuid = new SqlParameter("ApplicationGuid", ConfigurationManager.AppSettings["OMSApplicationGuid"].ToString());
            XmlSerializer serializer = new XmlSerializer(typeof(ContactRoleClaim));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, contactClaim);
            SqlParameter ContactRolesXML = new SqlParameter("ContactRolesXml", stringWriter.ToString());
            SqlParameter ClaimsRequestType = new SqlParameter("ClaimsRequestType", 1);
            SqlParameter[] paramArray = { contactGuid, ContactPCN, ApplicationGuid, ContactRolesXML, ClaimsRequestType };
            StringReader xmlReader = SqlHelper.ExecXMLReader("GetClaims", paramArray, Constants.OrganizationDB);
            serializer = new XmlSerializer(typeof(ContactClaim));
            ContactClaim response = serializer.Deserialize(xmlReader) as ContactClaim;
            if (userDetails.Organizations == null)
            {
                userDetails.Organizations = new List<ClaimOrganization>();
            }
            foreach (ContactClaimLocalContactClaimOrganizationClaims org in response.LocalContactClaim.OrganizationClaims)
            {
                ClaimOrganization objOrganization = new ClaimOrganization();
                objOrganization.PCN = org.PublicNumber;
                objOrganization.OrganizationName = org.LocalOrganizationClaim.InternalName;
                userDetails.Organizations.Add(objOrganization);
            }
        }

        /// <summary>
        /// ExtractRole splits the ebac based on the roleName and extract the "," seperated roles string
        /// </summary>
        /// <param name="ebac">string</param>
        /// <param name="roleName">OMCRoles or eMSLRoles or OrderRoles</param>
        /// <returns>string</returns>
        private static string ExtractRole(string ebac, string roleName)
        {
            string roleSet = string.Empty;
            if (!string.IsNullOrEmpty(ebac) && !string.IsNullOrEmpty(roleName))
            {
                if (ebac.ToUpper().Contains(roleName.ToUpper().Trim()))
                {
                    string[] separator = { roleName };
                    string[] result = ebac.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (result.Length > 0)
                    {
                        int itemIndex = result.Length > 1 ? 1 : 0;
                        roleSet = result[itemIndex].Trim().Remove(result[itemIndex].Trim().IndexOf(')'));
                        roleSet = roleSet.Trim().Substring(result[itemIndex].Trim().IndexOf('(') + 1, roleSet.Length - 1);
                    }
                }
            }
            return roleSet;
        }

        private static bool ParseEbac(string ebacValue, UserContext context)
        {
            if (ebacValue == "QuoteOnlyUser" || ebacValue == "+ 0|8 + 1|64 2|256")
            {
                ebacValue = "+ 0|8 + 1|64 2|448";
            }
            var resourcesConfigPath = HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["EBACRolespace_Path"];
            var roles = new Decompiler(resourcesConfigPath).Decompile(ebacValue);
            if (!string.IsNullOrEmpty(roles))
            {
                var roleSet = ExtractRole(roles, Constants.EBACOrderRoles);
                AddRolestoClaims(roleSet);
            }
            else
            {
                /* No Roles for User, re-direct to log-in page */
                return context.IsAuthorized = false;
            }
            var eval = new Evaluator();
            eval.SetUserRoles(ebacValue);
            var complier = new Compiler(resourcesConfigPath);
            if (eval.Evaluate(complier.Compile("OrderRoles (SuperAdmin)")) || eval.Evaluate(complier.Compile("OrderRoles (Submit)")) || eval.Evaluate(complier.Compile("OrderRoles (ROSubmitUser)")))
            {
                context.CanSubmit = true;
                context.IsReadonly = false;
            }
            else if (eval.Evaluate(complier.Compile("OrderRoles (Edit)")) || eval.Evaluate(complier.Compile("OrderRoles (ROEditUser)")))
            {
                context.IsReadonly = false;
            }
            return true;
        }
    }
}
