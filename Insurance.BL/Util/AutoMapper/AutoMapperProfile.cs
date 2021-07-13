using AutoMapper;
using Insurance.BL.Model;
using Insurance.DL.Entities;
using Insurance.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.BL.Util.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SetupDTOMap();
            InsuranceDTOMap();
            InsuranceDetailDTOMap();
            UserGroupDTOMap();
            UsersDTOMap();
        }

        private void SetupDTOMap()
        {
            CreateMap<Setup, SetupDTO>().IgnoreAllNonExisting();
            CreateMap<SetupDTO, Setup>().IgnoreAllNonExisting();
        }
        private void InsuranceDTOMap()
        {
            CreateMap<InsuranceInfo, InsuranceInfoDTO>().IgnoreAllNonExisting();
            CreateMap<InsuranceInfoDTO, InsuranceInfo>().IgnoreAllNonExisting();
            CreateMap<InsuranceInfoDetail, InsuranceInfoDetailDTO>().IgnoreAllNonExisting();
        }
        private void InsuranceDetailDTOMap()
        {
            CreateMap<InsuranceInfoDetail, InsuranceInfoDetailDTO>().IgnoreAllNonExisting();
            CreateMap<InsuranceInfoDetailDTO, InsuranceInfoDetail>().IgnoreAllNonExisting();
        }
        private void UserGroupDTOMap()
        {
            CreateMap<UserCreationDTO, Users>().IgnoreAllNonExisting();
            CreateMap<UserGroup, UserGroupDTO>().IgnoreAllNonExisting();
            CreateMap<UserGroupDTO, UserGroup>().IgnoreAllNonExisting();
        }
        private void UsersDTOMap()
        {
            CreateMap<Users, UsersDTO>().IgnoreAllNonExisting();
            CreateMap<UsersDTO, Users>().IgnoreAllNonExisting();
        }

    }
}
