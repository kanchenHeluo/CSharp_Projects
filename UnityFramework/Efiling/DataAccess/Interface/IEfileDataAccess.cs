

using Efiling.Common;
using System.Collections.Generic;

namespace Efiling.DataAccess.Interface
{
    public interface IEfileDataAccess
    {
        long SaveEfile(Efile efile);
        IEnumerable<Efile> GetEfileHistory(long id);
        EfileStatus UpdateEfileHistory(Efile efile);

    }
}
