using System.Collections.Generic;
using Efiling.DataAccess;
using Efiling.DataAccess.Interface;

namespace Efiling
{
    public class EfileHistory
    {
        private static IEfileDataAccess _db
        {
            get { return UnityConfig.Db; }
        }
        public static IEnumerable<Efile> GetStateEfileHistory(long id)
        {
            return _db.GetEfileHistory(id);
        }

        public static long SaveEfileHistory(Efile efile)
        {
            return _db.SaveEfile(efile);
        }
    }
}
