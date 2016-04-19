using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common
{
    public static class SessionExtensions    
    {

        public static void Set(this HttpSessionStateBase session, string key, Object obj)
        {
            session[key] = obj;
        }

        public static object Get(this HttpSessionStateBase session, string key)
        {
            if (session[key] == null)
            { return null; }
            return session[key];
        }

        public static void ClearSession(this HttpSessionStateBase session)
        {
            try           
            {
                //Clear the session variables
                session.Abandon();
                session.Clear();
                session.RemoveAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}