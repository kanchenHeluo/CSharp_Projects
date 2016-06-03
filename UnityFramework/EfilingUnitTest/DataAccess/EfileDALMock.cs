
using Efiling;
using Efiling.Common;
using Efiling.DataAccess.Interface;
using System.Collections.Generic;

namespace EfilingUnitTest.DataAccess
{
    public class EfileDALMock : IEfileDataAccess
    {
        public long SaveEfile(Efile efile)
        {
            return 3;
        }

        public IEnumerable<Efile> GetEfileHistory(long id)
        {
            Efile efile = new Efile();
            yield return efile;
        }

        public EfileStatus UpdateEfileHistory(Efile efile)
        {
            return EfileStatus.Created;
        }
    }
}
