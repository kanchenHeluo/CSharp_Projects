using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Web.UI.Common.Extensions;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;
using Web.UI.ServiceGateway.OrderServiceProxy;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Repositories.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<string, int>().ConvertUsing<IntTypeConverter>();
            Mapper.CreateMap<string, int?>().ConvertUsing<NullIntTypeConverter>();
            Mapper.CreateMap<string, decimal?>().ConvertUsing<NullDecimalTypeConverter>();
            Mapper.CreateMap<string, decimal>().ConvertUsing<DecimalTypeConverter>();
            Mapper.CreateMap<string, bool?>().ConvertUsing<NullBooleanTypeConverter>();
            Mapper.CreateMap<string, bool>().ConvertUsing<BooleanTypeConverter>();
            Mapper.CreateMap<string, Int64?>().ConvertUsing<NullInt64TypeConverter>();
            Mapper.CreateMap<string, Int64>().ConvertUsing<Int64TypeConverter>();
            Mapper.CreateMap<string, DateTime?>().ConvertUsing<NullDateTimeTypeConverter>();
            Mapper.CreateMap<string, DateTime>().ConvertUsing<DateTimeTypeConverter>();
            Mapper.CreateMap<VLLineItem, OpportunityLineItem>();
            Mapper.CreateMap<DomainItemResponse, DomainItem>();
            Mapper.CreateMap<AgreementSummary, Agreement>().ForMember(d => d.EndCustomerName, d => d.MapFrom(s => s.CustomerName))
                .ForMember(d =>d.EndCustomerNumber, d=>d.MapFrom(s =>s.EndCustomerPCN))
                .ForMember(d =>d.CountryName, d=>d.MapFrom(s =>s.CustomerLocationName))
                .ForMember(d =>d.PartnerNumber, d=>d.MapFrom(s=>s.BillToPCN));

            Mapper.CreateMap<AgreementDetail, Agreement>()
                .ForMember(d => d.PartnerNumber, d => d.MapFrom(s => s.BillToPCN))
                .ForMember(d => d.EndCustomerNumber, d => d.MapFrom(s => s.PrimaryPCN))
                .ForMember(d => d.EndCustomerName, d => d.MapFrom(s => s.CustomerName))
                .ForMember(d => d.ComplianceWindow, d => d.MapFrom(s => s.IsCompliant));

            Mapper.CreateMap<GetAllAgreements, Agreement>().ForMember(d => d.PartnerNumber, d => d.MapFrom(s => s.BillToPCN))
                .ForMember(d => d.ComplianceWindow, d => d.MapFrom(s => s.IsComplianceFlag))
                .ForMember(d => d.EndCustomerNumber, d => d.MapFrom(s => s.PrimaryPCN))
                .ForMember(d => d.EndCustomerName, d => d.MapFrom(s => s.CustomerName))
                .ForMember(d => d.PurchaseAgreementNumber, d => d.MapFrom(s => s.AgreementNumber));

            Mapper.CreateMap<StepupSearch, OpportunityLineItem>()
                .ForMember(d => d.PurchaseAgreementId, d => d.MapFrom(s => s.POAgreementId))
                .ForMember(d => d.PurchaseAgreementNumber, d => d.MapFrom(s => s.POAgreementNumber))
                .ForMember(d => d.ReferenceId, d => d.MapFrom(s => s.ReferencePOLIId));
         
            Mapper.CreateMap<OrderHeader, DraftOrderAttrs>()
                .ForMember(d => d.Id, d => d.MapFrom(s => s.DraftOrderId))
                .ForMember(d => d.ShipToId, d => d.MapFrom(s => s.PurchaseOrderShipToId))
                .ForMember(d => d.CreatedBy, d => d.MapFrom(s => s.CreatedUser))
                .ForMember(d => d.PurchaseOrderType, d => d.MapFrom(s => s.PurchaseOrderTypeCode))
                .ForMember(d => d.AssignedTo, d => d.MapFrom(s => s.AssignedTo))
                .ReverseMap()
                .ForMember(d => d.DraftOrderId, d => d.MapFrom(s => s.Id))
                .ForMember(d => d.PurchaseOrderShipToId, d => d.MapFrom(s => s.ShipToId))
                .ForMember(d => d.CreatedUser, d => d.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.PurchaseOrderTypeCode, d => d.MapFrom(s => s.PurchaseOrderType))
                .ForMember(d=>d.CorrelationId, d=>d.MapFrom(s=>s.CorrelationId))
                .ForMember(d => d.AssignedTo, d => d.MapFrom(s => s.AssignedTo))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<PricingDrivers, DraftOrderLineItem>()
                .ReverseMap()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<Shipment, DraftOrderShipment>()
                .ForMember(d => d.ShipToId, d => d.MapFrom(s => s.PurchaseOrderShipToId))
                .ForMember(d => d.ShipToFirstName, d => d.MapFrom(s => s.ContactFirstName))
                .ForMember(d => d.ShipToLastName, d => d.MapFrom(s => s.ContactLastName))
                .ForMember(d => d.ShipToAddressLine1, d => d.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.ShipToAddressLine2, d => d.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.ShipToAddressLine3, d => d.MapFrom(s => s.AddressLine3))
                .ForMember(d => d.ShipToAddressLine4, d => d.MapFrom(s => s.AddressLine4))
                .ForMember(d => d.ShipToOrganizationName, d => d.MapFrom(s => s.OrganizationName))
                .ForMember(d => d.ShipToPostalCode, d => d.MapFrom(s => s.PostalCode))
                .ForMember(d => d.ShipToCity, d => d.MapFrom(s => s.City))
                .ForMember(d => d.ShipToStateProvince, d => d.MapFrom(s => s.StateProvince))
                .ForMember(d => d.ShipToCountryCode, d => d.MapFrom(s => s.CountryCode))
                .ForMember(d => d.ShipToContactPhoneNumber, d => d.MapFrom(s => s.ContactPhoneNumber))
                .ForMember(d => d.ShipToContactEMailAddress, d => d.MapFrom(s => s.ContactEmailAddress))
                .ForMember(d => d.ShipToContactFaxNumber, d => d.MapFrom(s => s.ContactFaxNumber))
                .ForMember(d => d.CorrespondenceLanguageCode, d => d.MapFrom(s => s.CorrespondenceLanguageCode))
                .ForMember(d => d.LicenseProgramCode, d => d.MapFrom(s => s.LicenseProgramCode))

                //TODO: created by user, modified user, , CreatedDate, LastModifiedDate
                //.ForMember(d => d.CreatedBy, d => d.MapFrom(s => s.CreatedByUser))

                .ReverseMap()
                .ForMember(d => d.PurchaseOrderShipToId, d => d.MapFrom(s => s.ShipToId))
                .ForMember(d => d.ContactFirstName, d => d.MapFrom(s => s.ShipToFirstName))
                .ForMember(d => d.ContactLastName, d => d.MapFrom(s => s.ShipToLastName))
                .ForMember(d => d.AddressLine1, d => d.MapFrom(s => s.ShipToAddressLine1))
                .ForMember(d => d.AddressLine2, d => d.MapFrom(s => s.ShipToAddressLine2))
                .ForMember(d => d.AddressLine3, d => d.MapFrom(s => s.ShipToAddressLine3))
                .ForMember(d => d.AddressLine4, d => d.MapFrom(s => s.ShipToAddressLine4))
                .ForMember(d => d.OrganizationName, d => d.MapFrom(s => s.ShipToOrganizationName))
                .ForMember(d => d.PostalCode, d => d.MapFrom(s => s.ShipToPostalCode))
                .ForMember(d => d.City, d => d.MapFrom(s => s.ShipToCity))
                .ForMember(d => d.StateProvince, d => d.MapFrom(s => s.ShipToStateProvince))
                .ForMember(d => d.CountryCode, d => d.MapFrom(s => s.ShipToCountryCode))
                .ForMember(d => d.ContactPhoneNumber, d => d.MapFrom(s => s.ShipToContactPhoneNumber))
                .ForMember(d => d.ContactEmailAddress, d => d.MapFrom(s => s.ShipToContactEMailAddress))
                .ForMember(d => d.ContactFaxNumber, d => d.MapFrom(s => s.ShipToContactFaxNumber))
                .ForMember(d => d.CorrespondenceLanguageCode, d => d.MapFrom(s => s.CorrespondenceLanguageCode))
                .ForMember(d => d.LicenseProgramCode, d => d.MapFrom(s => s.LicenseProgramCode))
                //TODO: created by user, modified user, CreatedDate, LastModifiedDate

                .IgnoreAllNonExisting();
          

            Mapper.CreateMap<OrderLineItem, DraftOrderLineItem>()
                .ForMember(d => d.PricingCountryCode, d => d.MapFrom(s => s.PricingDrivers.PricingCountryCode))
                .ForMember(d => d.PricingCurrencyCode, d => d.MapFrom(s => s.PricingDrivers.PricingCurrencyCode))

                .ForMember(d => d.ListPrice, d => d.MapFrom(s => s.PriceAtNewLevel.ListPrice))
                .ForMember(d => d.NetPrice, d => d.MapFrom(s => s.PriceAtNewLevel.NetPrice))
                .ForMember(d => d.OfferingLevel, d => d.MapFrom(s => s.PriceAtNewLevel.OfferingLevel))
                .ForMember(d => d.Points, d => d.MapFrom(s => s.PriceAtNewLevel.Points))
                .ForMember(d => d.SystemPrice, d => d.MapFrom(s => s.PriceAtNewLevel.SystemPrice))


                .ForMember(d => d.QuantityFrom, d => d.MapFrom(s => s.OrderQuantity))
                .ForMember(d => d.QuantityTo, d => d.MapFrom(s => s.QuantityOrdered))
                .ForMember(d => d.UsageCountry, d => d.MapFrom(s => s.UsageCountryCode))
                .ForMember(d => d.BillingOptionCode, d => d.MapFrom(s => s.BillingOption))
                .ForMember(d => d.PurchaseUnitQuantity, d => d.MapFrom(s => s.PurchaseUnitQuantity))

                .ReverseMap()
                .ForMember(d => d.OrderQuantity, d => d.MapFrom(s => s.QuantityFrom))
                .ForMember(d => d.QuantityOrdered, d => d.MapFrom(s => s.QuantityTo))
                .ForMember(d => d.UsageCountryCode, d => d.MapFrom(s => s.UsageCountry))
                .ForMember(d => d.BillingOption, d => d.MapFrom(s => s.BillingOptionCode))
                .ForMember(d => d.PurchaseUnitQuantity, d => d.MapFrom(s => s.PurchaseUnitQuantity))

                .ForMember(d => d.PricingDrivers, d => d.MapFrom(s => new PricingDrivers()
                {
                    PricingCountryCode = s.PricingCountryCode,
                    PricingCurrencyCode = s.PricingCurrencyCode
                }))

                .ForMember(d => d.PriceAtNewLevel, d => d.MapFrom(s => new PriceAtNewLevel()
                {                                                                          
                    ListPrice = s.ListPrice,
                    NetPrice = s.NetPrice,
                    OfferingLevel = s.OfferingLevel,
                    Points = s.Points,
                    SystemPrice = s.SystemPrice
                }))

                .IgnoreAllNonExisting();

            Mapper.CreateMap<OrderLineItem, VLOrderLineItem>()
                .ForMember(d => d.IsOlsSku, d => d.MapFrom(s => s.IsOlsItem))  
                .ForMember(d => d.ItemID, d => d.MapFrom(s => s.ItemId))
                .ForMember(d => d.BillingOptionCode, d => d.MapFrom(s => s.BillingOption))
                .ForMember(d => d.ItemTypeCode, d => d.MapFrom(s => s.LineItemType))
                .ForMember(d => d.POLIUsageDate, d => d.MapFrom(s => s.POLIUsageDateTime))
                .ForMember(d => d.POLineItemId, d => d.MapFrom( s=> s.POLineItemId))
                .ForMember(d => d.SystemPrice, d => d.MapFrom(s => s.UnitPrice))
                //.ForMember(d => d.Quantity, d => d.MapFrom(s => s.OrderQuantity))
                .ReverseMap()
                .ForMember(d => d.IsOlsItem, d => d.MapFrom(s => s.IsOlsSku))
                .ForMember(d => d.ItemId, d => d.MapFrom(s => s.ItemID))
                .ForMember(d => d.BillingOption, d => d.MapFrom(s => s.BillingOptionCode))
                .ForMember(d => d.LineItemType, d => d.MapFrom(s => s.ItemTypeCode))
                .ForMember(d => d.POLIUsageDateTime, d => d.MapFrom(s => s.POLIUsageDate))
                .ForMember(d => d.POLineItemId, d => d.MapFrom(s => s.POLineItemId))
                .ForMember(d => d.UnitPrice, d => d.MapFrom(s => s.SystemPrice))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<OrderLineItem, VLLineItem>()
                .ForMember(d => d.BillingOptionCode, d => d.MapFrom(s => s.BillingOption))               
                .ForMember(d => d.AvailableQuantity, d => d.MapFrom(s => s.QuantityOrdered))
                .ForMember(d => d.OrderQuantity, d => d.MapFrom(s => s.QuantityOrdered))
                .ForMember(d => d.ReferencePOLineItemIds, d => d.MapFrom(s => s.ReferencePOLIds.ToArray()))
                .ForMember(d => d.LineItemGuid,d=>d.MapFrom(s=>Guid.NewGuid()))
                //.ForMember(d => d.LineItemType, d => d.MapFrom(s=>"RTRDTR"))
                .ForMember(d => d.POLIUsageDateTime, d => d.MapFrom(s => s.POLIUsageDateTime))
                .ForMember(d => d.Comments, opt => opt.Ignore())
                .ForMember(d => d.ItemDetails, d => d.MapFrom(s => new VLItemInfo()
                {
                    PoolCode = s.PoolCode,
                    Name = s.ItemName,
                    Number = s.PartNumber,
                    ProductFamilyCode = s.ProductFamilyCode,
                    ProductTypeCode = s.ProductTypeCode,
                    Id = s.ItemId
                }))
               .ForMember(d => d.PricingDrivers, d => d.MapFrom(s => new []{new PricingDriver()
                {
                    ProgramOfferingCode=s.ProgramOfferingCode,
                    ListPrice=s.UnitPrice,
                    PricingCountryCode=s.PricingDrivers.PricingCountryCode,
                    PricingCurrencyCode=s.PricingDrivers.PricingCurrencyCode,
                    PurchaseUnitCode=s.PurchaseUnitTypeCode
                }}))
           
                .IgnoreAllNonExisting();

            Mapper.CreateMap<OrderHeader, VLHeader>()
                .ForMember(d => d.UsagePeriodDate, d => d.MapFrom(s => s.UsageDate))
                .ForMember(d => d.PurchaseOrderTypeCode, d => d.MapFrom(s => s.PurchaseOrderTypeCode))
                .ForMember(d => d.Name, d => d.MapFrom(s => s.OrderName))
                .ForMember(d => d.Comments, opt => opt.Ignore())
                .IgnoreAllNonExisting();

            Mapper.CreateMap<OrderItem, OrderLineItem>()
                .ForMember(s => s.ItemId, opt => opt.MapFrom(d => d.ItemID))
                .ReverseMap()
                .ForMember(s => s.ItemID, opt => opt.MapFrom(d => d.ItemId))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ProductPriceDetail, OrderLineItem>()
                .ReverseMap()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ProductBase, OrderLineItem>()
                .ReverseMap()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ProductsList, OrderLineItem>()
                .ReverseMap()
                .IgnoreAllNonExisting();

            Mapper.CreateMap<SearchPurchaseOpportunitiesResponse, OrderLineItem>()
               .ReverseMap()
               .IgnoreAllNonExisting();

            Mapper.CreateMap<CompletedOrder, OrderHeader>()
                .ForMember(d => d.TotalExtendedAmount, opt => opt.MapFrom(s => s.ExtendedAmount))
                .ForMember(d => d.DirectCustomerNumber, opt => opt.MapFrom(s => s.DirectCustomerBillToNumber))
                .ForMember(d => d.IndirectCustomerNumber, opt => opt.MapFrom(s => s.IndirectCustomerNumber))
               .ForMember(d => d.Id, opt => opt.MapFrom(s => s.PurchaseOrderId))
               .ForMember(d=>d.EndCustomerName, opt => opt.MapFrom(s => s.CustomerName))
               .ForMember(d=>d.EndCustomerNumber, opt => opt.MapFrom(s => s.PublicCustomerNumber))
               .ForMember(d => d.Comments, opt => opt.MapFrom(s => new []
               {
                   new DraftOrderComment()
                   {
                       Comment = s.Comments,
                       Category = "USR"
                   }
               }))
               .ReverseMap()
               .IgnoreAllNonExisting();

            Mapper.CreateMap<SearchAgreements, Agreement>().
                ForMember(d=>d.StartEffectiveDate,opt => opt.MapFrom(s => s.AgreementStartDate))
                .ForMember(d => d.EndEffectiveDate, opt => opt.MapFrom(s => s.AgreementEndDate))
                .ForMember(d => d.AgreementNumber, opt => opt.MapFrom(s => s.POAgreementNumber))
                .ForMember(d => d.AgreementId, opt => opt.MapFrom(s => s.POAgreementId))
               .ForMember(d => d.EndCustomerName, opt => opt.MapFrom(s => s.EndCustomerName))
               .ForMember(d => d.EndCustomerNumber, opt => opt.MapFrom(s => s.EndCustomerNumber))
               
              .ReverseMap()
              .IgnoreAllNonExisting();
            Mapper.CreateMap<SaveDraftOrderShipmentPortalResponse, SaveShipmentAddressResult>().
                ForMember(d => d.ShipToId, opt => opt.MapFrom(s => s.ShipToId))
                .ForMember(d => d.Errors, opt => opt.MapFrom(s => s.Errors))
                .IgnoreAllNonExisting();

        }
        #region AutoMapTypeConverters
        // Automap type converter definitions for 
        // int, int?, decimal, decimal?, bool, bool?, Int64, Int64?, DateTime
        // Automapper string to int?
        private class NullIntTypeConverter : TypeConverter<string, int?>
        {
            protected override int? ConvertCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    int result;
                    return Int32.TryParse(source, out result) ? (int?)result : null;
                }
            }
        }
        // Automapper string to int
        private class IntTypeConverter : TypeConverter<string, int>
        {
            protected override int ConvertCore(string source)
            {
                if (source == null)
                    return 0123;
                // throw new MappingException("null string value cannot convert to non-nullable return type.");
                else
                    return Int32.Parse(source);
            }
        }
        // Automapper string to decimal?
        private class NullDecimalTypeConverter : TypeConverter<string, decimal?>
        {
            protected override decimal? ConvertCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    decimal result;
                    return Decimal.TryParse(source, out result) ? (decimal?)result : null;
                }
            }
        }
        // Automapper string to decimal
        private class DecimalTypeConverter : TypeConverter<string, decimal>
        {
            protected override decimal ConvertCore(string source)
            {
                if (source == null)
                    return 0123;
                //   throw new MappingException("null string value cannot convert to non-nullable return type.");
                else
                    return Decimal.Parse(source);
            }
        }
        // Automapper string to bool?
        private class NullBooleanTypeConverter : TypeConverter<string, bool?>
        {
            protected override bool? ConvertCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    bool result;
                    return Boolean.TryParse(source, out result) ? (bool?)result : null;
                }
            }
        }
        // Automapper string to bool
        private class BooleanTypeConverter : TypeConverter<string, bool>
        {
            protected override bool ConvertCore(string source)
            {
                bool result = false;
                if (source == null)
                    return false;
                if (!Boolean.TryParse(source, out result))
                        {
                    result = source == "1" ? true : false;
                        }
                        return result;
              
            }
        }
        // Automapper string to Int64?
        private class NullInt64TypeConverter : TypeConverter<string, Int64?>
        {
            protected override Int64? ConvertCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    Int64 result;
                    return Int64.TryParse(source, out result) ? (Int64?)result : null;
                }
            }
        }
        // Automapper string to Int64
        private class Int64TypeConverter : TypeConverter<string, Int64>
        {
            protected override Int64 ConvertCore(string source)
            {
                if (source == null)
                    return 0123;
                //  throw new MappingException("null string value cannot convert to non-nullable return type.");
                else
                    return Int64.Parse(source);
            }
        }
        // Automapper string to DateTime?
        // In our case, the datetime will be a JSON2.org datetime
        // Example: "/Date(1288296203190)/"
        private class NullDateTimeTypeConverter : TypeConverter<string, DateTime?>
        {
            protected override DateTime? ConvertCore(string source)
            {
                if (source == null)
                    return null;
                else
                {
                    DateTime result;
                    return DateTime.TryParse(source, out result) ? (DateTime?)result : null;
                }
            }
        }
        // Automapper string to DateTime
        private class DateTimeTypeConverter : TypeConverter<string, DateTime>
        {
            protected override DateTime ConvertCore(string source)
            {
                if (source == null)
                    return DateTime.Now;
                // throw new MappingException("null string value cannot convert to non-nullable return type.");
                else
                    return DateTime.Parse(source);
            }
        }
        #endregion

    }
}