using Efiling.Common;
using System.Collections.Generic;
using Efiling.DataAccess.Interface;
using Efiling.DataAccess;
using System.Linq;

namespace Efiling
{
    public partial class EfileSettings
    {
        private static IEfileDataAccess Db
        {
            get { return UnityConfig.Db; }
        }

        private static readonly Efile De9 = new Efile { State = USState.CA, IsEpay = false, Form = EfileForms.DE9 };
        private static readonly Efile S1 = new Efile { State = USState.NY, IsEpay = true, Form = EfileForms.S1 };

        private static readonly IList<Efile> EfileConfigs = new List<Efile>
        {
            De9,
            S1
        };

        public static readonly IDictionary<string, Efile> ConfigDict = EfileConfigs.ToDictionary(e => e.DataKey.ToUpper());
    }
}
