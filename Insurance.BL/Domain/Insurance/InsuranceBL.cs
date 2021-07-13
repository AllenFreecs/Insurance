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

namespace Insurance.Insurance.BL.insurance
{
    public class InsuranceBL : IInsuranceBL
    {
        private InsuranceDBContext _dbcontext;
        public InsuranceBL(InsuranceDBContext InsuranceContext)
        {
            _dbcontext = InsuranceContext;
        }

        public async Task<GlobalResponseDTO> DeleteInsurance(IEnumerable<int> IDs)
        {
            try
            {
                foreach (var item in IDs)
                {
                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {

                            var data = _dbcontext.InsuranceInfo.Where(c => c.ID == item).SingleOrDefault();
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

        public async Task<int?> GetInsuranceCount(InsuranceCountRequest model)
        {
            try
            {
                var data = await _dbcontext.InsuranceInfo.Where(c => c.IsActive == true).OrderBy(o => o.CreatedDate).ToListAsync();

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    data = data.Where(c => c.LastName.Contains(model.LastName)).ToList();
                }
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    data = data.Where(c => c.FirstName.Contains(model.FirstName)).ToList();
                }
                if (!string.IsNullOrEmpty(model.MiddleName))
                {
                    data = data.Where(c => c.FirstName.Contains(model.MiddleName)).ToList();
                }
                if (model.StartDate.HasValue && model.EndDate.HasValue)
                {
                    data = data.Where(c => c.BirthDate >= model.StartDate.Value && c.BirthDate <= model.EndDate.Value).ToList();
                }
                if (model.BasicSalaryStart.HasValue && model.BasicSalaryEnd.HasValue)
                {
                    data = data.Where(c => c.BasicSalary >= model.BasicSalaryStart && c.BasicSalary <= model.BasicSalaryEnd.Value).ToList();
                }

                return data.Count();
               
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<InsuranceInfoDTO> GetInsuranceData(int ID)
        {
            try
            {
                var data = await _dbcontext.InsuranceInfo.Where(c => c.ID == ID && c.IsActive == true).SingleOrDefaultAsync();
                var detail = await _dbcontext.InsuranceInfoDetail.Where(c => c.ID_InsuranceInfo == ID).ToListAsync();


                var Insurancedata = Mapper.Map<InsuranceInfo, InsuranceInfoDTO>(data);
                Insurancedata.InsuranceInfoDetail = Mapper.Map<IEnumerable<InsuranceInfoDetail>, IEnumerable<InsuranceInfoDetailDTO>>(detail);


                return Insurancedata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<IEnumerable<InsuranceInfoDTO>> GetInsuranceList(InsurancePageRequest paging)
        {
            try
            {
                var data = await _dbcontext.InsuranceInfo.Where(c => c.IsActive == true).OrderBy(o=> o.CreatedDate).ToListAsync();

                if (!string.IsNullOrEmpty(paging.LastName)) {
                    data = data.Where(c => c.LastName.Contains(paging.LastName)).ToList();
                }
                if (!string.IsNullOrEmpty(paging.FirstName))
                {
                    data = data.Where(c => c.FirstName.Contains(paging.FirstName)).ToList();
                }
                if (!string.IsNullOrEmpty(paging.MiddleName))
                {
                    data = data.Where(c => c.MiddleName.Contains(paging.MiddleName)).ToList();
                }
                if (paging.StartDate.HasValue && paging.EndDate.HasValue)
                {
                    data = data.Where(c => c.BirthDate >= paging.StartDate.Value && c.BirthDate <= paging.EndDate.Value).ToList();
                }
                if (paging.BasicSalaryStart.HasValue && paging.BasicSalaryEnd.HasValue)
                {
                    data = data.Where(c => c.BasicSalary >= paging.BasicSalaryStart && c.BasicSalary <= paging.BasicSalaryEnd.Value).ToList();
                }

                var Insurancedata = Mapper.Map<IEnumerable<InsuranceInfo>, IEnumerable<InsuranceInfoDTO>>(data);

                Insurancedata = Insurancedata.Skip((paging.Page - 1) * paging.Pagesize).Take(paging.Pagesize).ToList();
                return Insurancedata;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        public async Task<GlobalResponseDTO> SaveInsurance(InsuranceInfoDTO model)
        {
            try
            {
                int ID;
                //trim
                model.FirstName = model.FirstName.TrimStart().TrimEnd().Replace("  ", " ");
                model.LastName = model.LastName.TrimStart().TrimEnd().Replace("  ", " ");
                model.MiddleName = model.MiddleName.TrimStart().TrimEnd().Replace("  ", " ");

                using (var transaction = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.ID != 0)
                        {

                            
                            var data = Mapper.Map<InsuranceInfoDTO, InsuranceInfo>(model);
                            data.IsActive = true;

                            ////delete dup records
                            var sql = string.Format("Delete from InsuranceInfoDetail where ID_InsuranceInfo = {0}", model.ID);
                            _dbcontext.Database.ExecuteSqlCommand(sql);

                            _dbcontext.Entry(data).State = EntityState.Modified;
                            _dbcontext.SaveChanges();
                            ID = data.ID;

                        }
                        else
                        {
                            //Check if name exist
                            var entry = await _dbcontext.InsuranceInfo.Where(c => c.FirstName.ToLower() == model.FirstName.ToLower()
                            && c.MiddleName.ToLower() == model.MiddleName.ToLower() && c.LastName.ToLower() == model.LastName.ToLower()
                            ).CountAsync();
                            if (entry != 0)
                            {
                                return new GlobalResponseDTO() { IsSuccess = false, Message = "User already in the system" };
                            }
                            var data = Mapper.Map<InsuranceInfoDTO, InsuranceInfo>(model);
                            _dbcontext.Entry(data).State = EntityState.Added;
                            _dbcontext.SaveChanges();
                            ID = data.ID;
                        }
                       
                        transaction.Commit();

                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        return new GlobalResponseDTO() { IsSuccess = false, Message = "Server processes error" };
                        throw;
                    }
                }

                var setup = await _dbcontext.Setup.SingleOrDefaultAsync();

                bool WithinRange = WithinAllowedRange(model.BirthDate.Value, setup.MinAgeLimit.Value, setup.MaxAgeLimit.Value);

                for (int i = setup.MinimumRange.Value; i <= setup.MaximumRange; i += setup.Increments.Value)
                {
                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {

                        try
                        {
                            decimal PendedAmount = WithinRange ? GetPendenAmount(model.BasicSalary.Value, i, setup.GuaranteedIssue.Value) : 0;
                            var detail = new InsuranceInfoDetailDTO()
                            {
                                Multiple = i,
                                BenefitsAmountQuotation = model.BasicSalary * i,
                                PendedAmount = PendedAmount,
                                Benefits = PendedAmount == 0 ? (model.BasicSalary * i).ToString() : "For Approval",
                                ID_InsuranceInfo = ID
                            };

                            var insuranceInfoDetail = Mapper.Map<InsuranceInfoDetailDTO, InsuranceInfoDetail>(detail);

                            _dbcontext.Entry(insuranceInfoDetail).State = EntityState.Added;
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

                return new GlobalResponseDTO() { IsSuccess = true, Message = "Data has been saved." , ID = ID};
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        private decimal GetPendenAmount(decimal BasicSalary , int Multiple,  decimal GuaranteedIssue ) {
            try
            {
                var amt = (BasicSalary * Multiple) - GuaranteedIssue;

                return amt < 0 ? 0 : amt ;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

        private bool WithinAllowedRange(DateTime BirthDate , int MinRange, int MaxRange )
        {
            try
            {
                int age = (DateTime.Now.Year - BirthDate.Date.Year);

                if (age >= MinRange && age <= MaxRange)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                throw new Exception("Server processes error", ex);
            }
        }

    }
}
