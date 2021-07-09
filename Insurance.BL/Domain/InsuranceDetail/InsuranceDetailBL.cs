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

namespace Insurance.Insurance.BL.insurancedetail
{
    public class InsuranceDetailBL : IInsuranceDetailBL
    {
        private InsuranceDBContext _dbcontext;
        public InsuranceDetailBL(InsuranceDBContext InsuranceContext)
        {
            _dbcontext = InsuranceContext;
        }

        public async Task<GlobalResponseDTO> DeleteInsuranceDetail(IEnumerable<int> IDs)
        {
            try
            {
                foreach (var item in IDs)
                {
                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {

                            var data = _dbcontext.InsuranceInfoDetail.Where(c => c.ID == item).SingleOrDefault();
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

        public async Task<InsuranceInfoDetailDTO> GetInsuranceDetailData(int ID)
        {
            try
            {
                var data = await _dbcontext.InsuranceInfoDetail.Where(c => c.ID == ID && c.IsActive == true).SingleOrDefaultAsync();
                var InsuranceDetaildata = Mapper.Map<InsuranceInfoDetail, InsuranceInfoDetailDTO>(data);

                return InsuranceDetaildata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<IEnumerable<InsuranceInfoDetailDTO>> GetInsuranceDetailList(PageRequest paging)
        {
            try
            {
                var data = await _dbcontext.InsuranceInfoDetail.Where(c => c.IsActive == true).ToListAsync();
                var InsuranceDetaildata = Mapper.Map<IEnumerable<InsuranceInfoDetail>, IEnumerable<InsuranceInfoDetailDTO>>(data);

                InsuranceDetaildata = InsuranceDetaildata.Skip((paging.Page - 1) * paging.Pagesize).Take(paging.Pagesize).ToList();
                return InsuranceDetaildata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> SaveInsuranceDetail(InsuranceInfoDetailDTO model)
        {
            try
            {
                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.ID != 0)
                        {

                            var data = Mapper.Map<InsuranceInfoDetailDTO, InsuranceInfoDetail>(model);
                            _dbcontext.Entry(data).State = EntityState.Modified;

                        }
                        else
                        {
                            var data = Mapper.Map<InsuranceInfoDetailDTO, InsuranceInfoDetail>(model);
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
