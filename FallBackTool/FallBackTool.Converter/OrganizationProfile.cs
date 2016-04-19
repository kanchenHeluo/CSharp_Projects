using AutoMapper;
using FallBackTool.Converter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Converter
{
    public class OrganizationProfile : Profile
    {
        protected override void Configure()
        {
            //SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            //DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            
            //maps
            CreateMap<ContactDBModel, MPCContactCRMModel>()
                .ForMember(dest => dest.Address1_StateOrProvince, opt => opt.MapFrom(src => src.address1_county))
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.personnelnumber))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.ActionCode));

            CreateMap<ContactDBModel, OPRContactCRMModel>()
                .ForMember(dest => dest.Address1_Country, opt => opt.MapFrom(src => src.workingPositionCountry))
                .ForMember(dest => dest.Adx_username, opt => opt.MapFrom(src => src.domain_alias))
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => Guid.Empty))
                .ForMember(dest => dest.EMailAddress2, opt => opt.MapFrom(src => src.secondaryEmailAddress))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.personnelnumber))
                .ForMember(dest => dest.ops_CompanyCode, opt => opt.MapFrom(src => src.CompanyCode))
                .ForMember(dest => dest.ops_CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.ActionCode));
                
        }
    } 
}
