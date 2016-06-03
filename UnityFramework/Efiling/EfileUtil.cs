using Efiling.DataAccess;
using Efiling.DataAccess.Interface;
using Efiling.Common;
using DotNetUtils;

namespace Efiling
{
    public static partial class EfileUtil
    {
        
        private static IEfileService _svc
        {
            get { return UnityConfig.Service; }
        }

        public static Efile GetStateEfileDefinition(this USState state, string type)
        {
            return ((USState?)state).GetStateEfileDefinition(type);
        }

        public static Efile GetStateEfileDefinition(this USState? state, string type)
        {
            if (state.HasValue && !string.IsNullOrWhiteSpace(type))
            {
                var key = GetDataKey(state.Value, type).ToUpper();
                return EfileSettings.ConfigDict.GetOrDefault(key);
                
            }
            return null;
        }
        public static string GetDataKey(this Efile efile)
        {
            return GetDataKey(efile.State, efile.Form);
        }


        public static string GetDataKey(USState state, string type)
        {
            return "{"+state + "}_{"+type+"}" ;
        }
    }
}
