using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Web.Common.Extensions;
using Web.UI.Models;
using Web.UI.Common.Extensions;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;
using Agreement = Web.UI.Models.Agreement;

namespace Web.UI
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Repositories.AutoMapper.AutoMapperConfig.RegisterMappings();

            Mapper.CreateMap<Agreement, Repositories.DomainModels.Agreement>()
                .ForMember(d => d.PricingCountryCode, opt => opt.MapFrom(s => s.CountryCode))
                .ForMember(d => d.PurchaseAgreementNumber, opt => opt.MapFrom(s => s.POAgreementNumber))
                .ReverseMap()
                .ForMember(s => s.CountryCode, opt => opt.MapFrom(d => d.PricingCountryCode))
                .ForMember(d=>d.ComplianceStatus,opt =>opt.MapFrom(s=>s.IsCompliant))
                .ForMember(d => d.POAgreementNumber, opt => opt.MapFrom(s => s.PurchaseAgreementNumber))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<OpportunityLineItem, OrderLineItem>()
                .ReverseMap()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<OpportunityLineItem, LineItem >()
                .ForMember(d => d.ItemName, opt => opt.MapFrom(s => s.ItemDetails.Name))
                .ForMember(d => d.PartNumber, opt => opt.MapFrom(s => s.ItemDetails.Number))
                .ForMember(d => d.OriginalItemName, opt => opt.MapFrom(s => s.ParentItemName))
                .ForMember(d => d.OriginalPartNumber, opt => opt.MapFrom(s => s.ParentPartNumber))
                .ForMember(d => d.OriginalItemId, opt => opt.MapFrom(s => s.ParentItemId))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<Order, OrderHeader>()
                .ReverseMap()
                .ForMember(s => s.UserComment, opt => opt.MapFrom(d => d.Comments.EmptyIfNull().FirstOrDefault(c => String.Equals(c.Category, "USR", StringComparison.OrdinalIgnoreCase)).TryGetNumber(m => m.Comment, null)))
                .ForMember(s => s.UserNotes, opt => opt.MapFrom(d => d.Comments.EmptyIfNull().FirstOrDefault(c => String.Equals(c.Category, "NTS", StringComparison.OrdinalIgnoreCase)).TryGetNumber(m => m.Comment, null)))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<LineItem, OrderLineItem>()
                .ForMember(s => s.POLineItemId, opt => opt.MapFrom(d => d.Id))
                .ForMember(s => s.ItemId, opt => opt.MapFrom(d => d.ProductId))
                
                .ForMember(s => s.OrderQuantity, opt => opt.MapFrom(d => d.QuantityAvailable))
                .ForMember(s => s.BillingOption, opt => opt.MapFrom(d => d.BillingOptionCode))
                
                .ForMember(s => s.PricingDrivers, opt => opt.MapFrom(d => new PricingDrivers()
                {
                    PricingCountryCode = d.PricingCountryCode,
                    PricingCurrencyCode = d.PricingCurrencyCode
                }))
                .ForMember(s => s.POLIUsageDateTime, opt => opt.MapFrom(d => d.POLIUsageDate))
                .ReverseMap()
                .ForMember(s => s.Id, opt => opt.MapFrom(d => d.POLineItemId))
                .ForMember(s => s.ProductId, opt => opt.MapFrom(d => d.ItemId))
                .ForMember(s => s.QuantityAvailable, opt => opt.MapFrom(d => d.OrderQuantity))
                .ForMember(s => s.BillingOptionCode, opt => opt.MapFrom(d => d.BillingOption))
                
                .ForMember(s => s.UserComment, opt => opt.MapFrom(d => d.Comments.EmptyIfNull().FirstOrDefault(c => String.Equals(c.Category, "USR", StringComparison.OrdinalIgnoreCase)).TryGetNumber(m => m.Comment, null)))
                .ForMember(s => s.PricingCountryCode, opt => opt.MapFrom(d => d.PricingDrivers.PricingCountryCode))
                .ForMember(s => s.PricingCurrencyCode, opt => opt.MapFrom(d => d.PricingDrivers.PricingCurrencyCode))
                .ForMember(s => s.POLIUsageDate, opt => opt.MapFrom(d=> d.POLIUsageDateTime))
                .IgnoreAllNonExisting();
        }

    }
}