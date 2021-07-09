using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.BL.Domain.RawSQL
{
    public interface IRawSQL
    {
        void ExcecuteRawSQL(string sql);
        IEnumerable<object> ExcecuteSQLList(string sql);
    }
}
