
using System.Collections.Generic;


namespace Efiling.DataAccess.Interface
{
    public interface IEfileService
    {
        Efile GenerateEfile(IDictionary<string, string> input);
    }
}
