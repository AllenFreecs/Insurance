using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog;
using Insurance.DL.Entities;
using Insurance.BL.Model;

namespace Insurance.Insurance.BL.setup
{
    public class SetupBL : ISetupBL
    {
        private InsuranceDBContext _dbcontext;
        public SetupBL(InsuranceDBContext InsuranceContext)
        {
            _dbcontext = InsuranceContext;
        }

        public async Task<GlobalResponseDTO> DeleteSetup(IEnumerable<int> IDs)
        {
            try
            {
                foreach (var item in IDs)
                {
                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {

                            var data = _dbcontext.Setup.Where(c => c.ID == item).SingleOrDefault();
                            data.IsActive = false;

                            _dbcontext.Entry(data).State = EntityState.Modified;
                            _dbcontext.SaveChanges();
                            transaction.Commit();

                        }
                        catch
                        {
                            transaction.Rollback();
                            return new GlobalResponseDTO() { IsSuccess = true, Message = "Server processes error" };
                            throw;
                        }
                    }
                }


                return new GlobalResponseDTO() { IsSuccess = true, Message = "Data has been deleted" };


            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<SetupDTO> GetSetupData(int ID)
        {
            try
            {
                var data = await _dbcontext.Setup.Where(c => c.ID == ID && c.IsActive == true).SingleOrDefaultAsync();
                var Setupdata = Mapper.Map<Setup, SetupDTO>(data);

                return Setupdata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<IEnumerable<SetupDTO>> GetSetupList(PageRequest paging)
        {
            try
            {
                var data = await _dbcontext.Setup.Where(c => c.IsActive == true).ToListAsync();
                var Setupdata = Mapper.Map<IEnumerable<Setup>, IEnumerable<SetupDTO>>(data);

                Setupdata = Setupdata.Skip((paging.Page - 1) * paging.Pagesize).Take(paging.Pagesize).ToList();
                return Setupdata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> SaveSetup(SetupDTO model)
        {
            try
            {
                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.Id != 0)
                        {

                            var data = Mapper.Map<SetupDTO, Setup>(model);
                            _dbcontext.Entry(data).State = EntityState.Modified;

                        }
                        else
                        {
                            var data = Mapper.Map<SetupDTO, Setup>(model);
                            _dbcontext.Entry(data).State = EntityState.Added;
                        }
                        _dbcontext.SaveChanges();
                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        return new GlobalResponseDTO() { IsSuccess = true, Message = "Server processes error" };
                        throw;
                    }
                }

                return new GlobalResponseDTO() { IsSuccess = true, Message = "Data has been saved." };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }
    }
}
