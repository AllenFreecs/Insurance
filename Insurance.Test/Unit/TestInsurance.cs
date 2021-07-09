using AutoMapper;
using Insurance.BL.Domain.RawSQL;
using Insurance.BL.Util.AutoMapper;
using Insurance.DL.Entities;
using Insurance.Insurance.BL.insurance;
using Insurance.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Insurance.Test.Unit
{
    public class TestInsurance
    {
        public TestInsurance()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }
        [Fact]
        public async void TestInsuranceSaveCase1() {
            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDBContext>();
            var context = new InsuranceDBContext(optionsBuilder.Options);

            var item = new InsuranceInfoDTO
            {
                ID = 0,
                LastName = "TestLastName",
                FirstName = "TestFirstName",
                MiddleName = "TestMiddleName",
                BasicSalary = 80000,
                BirthDate = new DateTime(1980, 06, 01)
            };
            

            var mockInsurance = new Mock<InsuranceDBContext>();
            mockInsurance.CallBase = true;
            var mockRawSQL = new Mock<InsuranceDBContext>();
            mockRawSQL.CallBase = true;

            var InsuranceBL = new InsuranceBL(mockInsurance.Object);
            var mockRawSQLBL = new RawSQLBL(mockRawSQL.Object);

            // Update Increment
            mockRawSQLBL.ExcecuteRawSQL("update Setup set Increments = 1");

            var data = await InsuranceBL.SaveInsurance(item);

          
            //mockRawSQLBL.ExcecuteRawSQL("update Setup set Increments = 1");

            var count = await context.InsuranceInfo.CountAsync();
            Assert.True(data.IsSuccess);
            Assert.True((count > 0));

            //Detail Expectation
            var ItemDetail = new List<InsuranceInfoDetail>()
            {
               new InsuranceInfoDetail  { 
                 ID = 1,
                 ID_InsuranceInfo = 1,
                 Multiple = 1,
                 BenefitsAmountQuotation = 80000,
                 PendedAmount = 30000,
                 Benefits = "For Approval"
               },
               new InsuranceInfoDetail  {
                 ID = 2,
                 ID_InsuranceInfo = 1,
                 Multiple = 2,
                 BenefitsAmountQuotation = 160000,
                 PendedAmount = 110000,
                 Benefits = "For Approval"
               },
               new InsuranceInfoDetail  {
                 ID = 3,
                 ID_InsuranceInfo = 1,
                 Multiple = 3,
                 BenefitsAmountQuotation = 240000,
                 PendedAmount = 190000,
                 Benefits = "For Approval"
               },
               new InsuranceInfoDetail  {
                 ID = 4,
                 ID_InsuranceInfo = 1,
                 Multiple = 4,
                 BenefitsAmountQuotation = 320000,
                 PendedAmount = 270000,
                 Benefits = "For Approval"
               },
               new InsuranceInfoDetail {
                 ID = 5,
                 ID_InsuranceInfo = 1,
                 Multiple = 5,
                 BenefitsAmountQuotation = 400000,
                 PendedAmount = 350000,
                 Benefits = "For Approval"
               }
            };

            var detail = await context.InsuranceInfoDetail.Where(c => c.ID_InsuranceInfo == 1).ToListAsync();
            //Assert.Equal(Item1Detail,detail1);


           //Assert Logic 1

            Assert.Collection(detail, info =>
            {
                Assert.Equal(info.ID, ItemDetail[0].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[0].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[0].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[0].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[0].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[0].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[1].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[1].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[1].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[1].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[1].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[1].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[2].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[2].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[2].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[2].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[2].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[2].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[3].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[3].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[3].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[3].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[3].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[3].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[4].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[4].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[4].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[4].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[4].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[4].Benefits);
            }
            );


            //Remove inserted test data and reseed the DB
            mockRawSQLBL.ExcecuteRawSQL(@"
            delete from InsuranceInfo
            delete from InsuranceInfoDetail
            DBCC CHECKIDENT('InsuranceInfo', RESEED, 0)
            DBCC CHECKIDENT('InsuranceInfoDetail', RESEED, 0)
            update Setup set Increments = 1
            ");

        }

        [Fact]
        public async void TestInsuranceSaveCase2()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDBContext>();
            var context = new InsuranceDBContext(optionsBuilder.Options);

            var item = new InsuranceInfoDTO
            {
                ID = 0,
                LastName = "TestLastName",
                FirstName = "TestFirstName",
                MiddleName = "TestMiddleName",
                BasicSalary = 80000,
                BirthDate = new DateTime(1980, 06, 01)
            };
           
            var mockInsurance = new Mock<InsuranceDBContext>();
            mockInsurance.CallBase = true;
            var mockRawSQL = new Mock<InsuranceDBContext>();
            mockRawSQL.CallBase = true;

            var InsuranceBL = new InsuranceBL(mockInsurance.Object);
            var mockRawSQLBL = new RawSQLBL(mockRawSQL.Object);

            //// Update Increment
            mockRawSQLBL.ExcecuteRawSQL("update Setup set Increments = 3");


            var data = await InsuranceBL.SaveInsurance(item);


            var count = await context.InsuranceInfo.CountAsync();
            Assert.True(data.IsSuccess);
            Assert.True((count > 0));

            //Detail Expectation
            var ItemDetail = new List<InsuranceInfoDetail>() {
                new InsuranceInfoDetail  {
                 ID = 1,
                 ID_InsuranceInfo = 1,
                 Multiple = 1,
                 BenefitsAmountQuotation = 80000,
                 PendedAmount = 30000,
                 Benefits = "For Approval"
               },
                new InsuranceInfoDetail  {
                 ID = 2,
                 ID_InsuranceInfo = 1,
                 Multiple = 4,
                 BenefitsAmountQuotation = 320000,
                 PendedAmount = 270000,
                 Benefits = "For Approval"
               },

            };

            var detail = await context.InsuranceInfoDetail.Where(c => c.ID_InsuranceInfo == 1).ToListAsync();
            //Assert.Equal(Item1Detail,detail1);

            //Assert Logic 2

            Assert.Collection(detail, info =>
            {
                Assert.Equal(info.ID, ItemDetail[0].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[0].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[0].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[0].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[0].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[0].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[1].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[1].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[1].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[1].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[1].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[1].Benefits);
            }
            );

            //Remove inserted test data and reseed the DB
            mockRawSQLBL.ExcecuteRawSQL(@"
            delete from InsuranceInfo
            delete from InsuranceInfoDetail
            DBCC CHECKIDENT('InsuranceInfo', RESEED, 0)
            DBCC CHECKIDENT('InsuranceInfoDetail', RESEED, 0)
            update Setup set Increments = 1
            ");

        }

        [Fact]
        public async void TestInsuranceSaveCase3()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDBContext>();
            var context = new InsuranceDBContext(optionsBuilder.Options);

            var item = new InsuranceInfoDTO
            {
                ID = 0,
                LastName = "TestLastName",
                FirstName = "TestFirstName",
                MiddleName = "TestMiddleName",
                BasicSalary = 5000,
                BirthDate = new DateTime(1980, 06, 01)
            };

            var mockInsurance = new Mock<InsuranceDBContext>();
            mockInsurance.CallBase = true;
            var mockRawSQL = new Mock<InsuranceDBContext>();
            mockRawSQL.CallBase = true;

            var InsuranceBL = new InsuranceBL(mockInsurance.Object);
            var mockRawSQLBL = new RawSQLBL(mockRawSQL.Object);

            //// Update Increment
            mockRawSQLBL.ExcecuteRawSQL("update Setup set Increments = 1,  GuaranteedIssue = 15000");


            var data = await InsuranceBL.SaveInsurance(item);


            var count = await context.InsuranceInfo.CountAsync();
            Assert.True(data.IsSuccess);
            Assert.True((count > 0));

            //Detail Expectation
            var ItemDetail = new List<InsuranceInfoDetail>()
            {
               new InsuranceInfoDetail  {
                 ID = 1,
                 ID_InsuranceInfo = 1,
                 Multiple = 1,
                 BenefitsAmountQuotation = 5000,
                 PendedAmount = 0,
                 Benefits = "5000"
               },
               new InsuranceInfoDetail  {
                 ID = 2,
                 ID_InsuranceInfo = 1,
                 Multiple = 2,
                 BenefitsAmountQuotation = 10000,
                 PendedAmount = 0,
                 Benefits = "10000"
               },
               new InsuranceInfoDetail  {
                 ID = 3,
                 ID_InsuranceInfo = 1,
                 Multiple = 3,
                 BenefitsAmountQuotation = 15000,
                 PendedAmount = 0,
                 Benefits = "15000"
               },
               new InsuranceInfoDetail  {
                 ID = 4,
                 ID_InsuranceInfo = 1,
                 Multiple = 4,
                 BenefitsAmountQuotation = 20000,
                 PendedAmount = 5000,
                 Benefits = "For Approval"
               },
               new InsuranceInfoDetail {
                 ID = 5,
                 ID_InsuranceInfo = 1,
                 Multiple = 5,
                 BenefitsAmountQuotation = 25000,
                 PendedAmount = 10000,
                 Benefits = "For Approval"
               }
            };

            var detail = await context.InsuranceInfoDetail.Where(c => c.ID_InsuranceInfo == 1).ToListAsync();
            //Assert.Equal(Item1Detail,detail1);

            //Assert Logic 3

            Assert.Collection(detail, info =>
            {
                Assert.Equal(info.ID, ItemDetail[0].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[0].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[0].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[0].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[0].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[0].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[1].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[1].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[1].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[1].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[1].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[1].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[2].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[2].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[2].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[2].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[2].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[2].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[3].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[3].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[3].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[3].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[3].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[3].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[4].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[4].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[4].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[4].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[4].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[4].Benefits);
            }
            );

            //Remove inserted test data and reseed the DB
            mockRawSQLBL.ExcecuteRawSQL(@"
            delete from InsuranceInfo
            delete from InsuranceInfoDetail
            DBCC CHECKIDENT('InsuranceInfo', RESEED, 0)
            DBCC CHECKIDENT('InsuranceInfoDetail', RESEED, 0)
            update Setup set Increments = 1, GuaranteedIssue = 50000
            ");

        }


        [Fact]
        public async void TestInsuranceSaveCase4()
        {
            var optionsBuilder = new DbContextOptionsBuilder<InsuranceDBContext>();
            var context = new InsuranceDBContext(optionsBuilder.Options);

            var item = new InsuranceInfoDTO
            {
                ID = 0,
                LastName = "TestLastName",
                FirstName = "TestFirstName",
                MiddleName = "TestMiddleName",
                BasicSalary = 50000,
                BirthDate = new DateTime(1950, 06, 01)
            };

            var mockInsurance = new Mock<InsuranceDBContext>();
            mockInsurance.CallBase = true;
            var mockRawSQL = new Mock<InsuranceDBContext>();
            mockRawSQL.CallBase = true;

            var InsuranceBL = new InsuranceBL(mockInsurance.Object);
            var mockRawSQLBL = new RawSQLBL(mockRawSQL.Object);

            //// Update Increment
            mockRawSQLBL.ExcecuteRawSQL("update Setup set Increments = 1");


            var data = await InsuranceBL.SaveInsurance(item);


            var count = await context.InsuranceInfo.CountAsync();
            Assert.True(data.IsSuccess);
            Assert.True((count > 0));

            //Detail Expectation
            var ItemDetail = new List<InsuranceInfoDetail>()
            {
               new InsuranceInfoDetail  {
                 ID = 1,
                 ID_InsuranceInfo = 1,
                 Multiple = 1,
                 BenefitsAmountQuotation = 50000,
                 PendedAmount = 0,
                 Benefits = "50000"
               },
               new InsuranceInfoDetail  {
                 ID = 2,
                 ID_InsuranceInfo = 1,
                 Multiple = 2,
                 BenefitsAmountQuotation = 100000,
                 PendedAmount = 0,
                 Benefits = "100000"
               },
               new InsuranceInfoDetail  {
                 ID = 3,
                 ID_InsuranceInfo = 1,
                 Multiple = 3,
                 BenefitsAmountQuotation = 150000,
                 PendedAmount = 0,
                 Benefits = "150000"
               },
               new InsuranceInfoDetail  {
                 ID = 4,
                 ID_InsuranceInfo = 1,
                 Multiple = 4,
                 BenefitsAmountQuotation = 200000,
                 PendedAmount = 0,
                 Benefits = "200000"
               },
               new InsuranceInfoDetail {
                 ID = 5,
                 ID_InsuranceInfo = 1,
                 Multiple = 5,
                 BenefitsAmountQuotation = 250000,
                 PendedAmount = 0,
                 Benefits = "250000"
               }
            };

            var detail = await context.InsuranceInfoDetail.Where(c => c.ID_InsuranceInfo == 1).ToListAsync();
            //Assert.Equal(Item1Detail,detail1);

            //Assert Logic 3

            Assert.Collection(detail, info =>
            {
                Assert.Equal(info.ID, ItemDetail[0].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[0].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[0].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[0].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[0].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[0].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[1].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[1].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[1].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[1].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[1].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[1].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[2].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[2].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[2].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[2].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[2].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[2].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[3].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[3].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[3].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[3].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[3].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[3].Benefits);
            },
            info =>
            {
                Assert.Equal(info.ID, ItemDetail[4].ID);
                Assert.Equal(info.ID_InsuranceInfo, ItemDetail[4].ID_InsuranceInfo);
                Assert.Equal(info.Multiple, ItemDetail[4].Multiple);
                Assert.Equal(info.PendedAmount, ItemDetail[4].PendedAmount);
                Assert.Equal(info.BenefitsAmountQuotation, ItemDetail[4].BenefitsAmountQuotation);
                Assert.Equal(info.Benefits, ItemDetail[4].Benefits);
            }
            );

            //Remove inserted test data and reseed the DB
            mockRawSQLBL.ExcecuteRawSQL(@"
            delete from InsuranceInfo
            delete from InsuranceInfoDetail
            DBCC CHECKIDENT('InsuranceInfo', RESEED, 0)
            DBCC CHECKIDENT('InsuranceInfoDetail', RESEED, 0)
            update Setup set Increments = 1, GuaranteedIssue = 50000
            ");

        }


    }
}
