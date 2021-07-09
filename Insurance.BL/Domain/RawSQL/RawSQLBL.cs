using Insurance.BL.Util.Helper;
using Insurance.DL.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.BL.Domain.RawSQL
{
    public class RawSQLBL : IRawSQL
    {
        private InsuranceDBContext _dbcontext;
        public RawSQLBL(InsuranceDBContext InsuranceContext)
        {
            _dbcontext = InsuranceContext;
        }

        public async void ExcecuteRawSQL(string sql)
        {
            try
            {
                using (var transaction = _dbcontext.Database.BeginTransaction())
                {

                    await _dbcontext.Database.ExecuteSqlCommandAsync(sql);
                    transaction.Commit();
                }
                //await _dbcontext.Database.ExecuteSqlCommandAsync(sql,null,);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public IEnumerable<object> ExcecuteSQLList(string sql)
        {
            try
            {
               var obj = _dbcontext.ExecuteRawQueryList<object>(sql);
               return obj;

            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }
    }
}
