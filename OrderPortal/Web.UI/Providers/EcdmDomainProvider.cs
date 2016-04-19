using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Web.UI.Data;
using Web.UI.Repositories.Models;
using System.Web.Mvc;
using Microsoft.IT.Licensing;
using Microsoft.IT.Licensing.ECDM.DomainData.Services.Client;
using Microsoft.IT.Licensing.Entity.DomainData;
using System.Data.Services.Client;
using System.Linq.Expressions;
using System.Web.Caching;
using Microsoft.IT.Licensing.DomainDataClient;
using System.Threading.Tasks;
using System.Collections;
using Telerik.Web.Mvc.Extensions;
using Web.UI.Common;
using Web.UI.Cache;
using Microsoft.Practices.Unity;

namespace Web.UI.Providers
{
    public class EcdmDomainProvider : IDomainDataProvider
    {
        DomainDataServiceContext _domainDataEntities = null;
        private static bool useEcdm = ConfigurationManager.AppSettings["UseEcdmDomainService"] == "true";
        private static bool isCountryLocalized = ConfigurationManager.AppSettings["isCountryLocalized"] == "true";
        private const string CountryCacheKey = "Country-";
        private static IDomainDataProvider _ecdmDomainDataFacade = null;
        private static Microsoft.IT.Licensing.DomainDataClient.DomainData domainDataClient = new Microsoft.IT.Licensing.DomainDataClient.DomainData();
        private const string LocaleCacheKey = "Locale";
        [Dependency]
        public ICacheProvider cache { get; set; }
        //InMemoryCacheProvider cache = new InMemoryCacheProvider();

        private static IDomainDataProvider EcdmDomainDataFacade
        {
            get
            {
                return _ecdmDomainDataFacade ?? (_ecdmDomainDataFacade = new EcdmDomainProvider());
            }
        }


        public EcdmDomainProvider()
        {
            string urlSetting = ConfigurationManager.AppSettings["EcdmDomainDataService"];
            if (null == urlSetting)
            {
                throw new ArgumentException("Ecdm Data Service Url cannot be null.");
            }

            _domainDataEntities = new DomainDataServiceContext(new Uri(urlSetting));
        }

        #region Public Methods

        /// <summary>
        /// Get Billing Option
        /// </summary>
        /// 
        public async Task<ICollection<Microsoft.IT.Licensing.Entity.DomainData.BillingOption>> GetBillingOptionsAsync(ICollection<string> productTypeCodes, ICollection<string> programCodes, ICollection<string> purchaseOrderTypes, int localeId)
        {

            List<BillingOption> output = new List<BillingOption>();
            List<string> filterList = new List<string>();
            foreach (string productTypeCode in productTypeCodes)
            {
                foreach (string programCode in programCodes)
                {
                    if (purchaseOrderTypes.Any())
                    {
                        foreach (var purchaseOrderType in purchaseOrderTypes)
                        {

                            List<string> productCodes = new List<string>();
                            productCodes.Add(productTypeCode);
                            List<string> progCode = new List<string>();
                            progCode.Add(programCode);
                            List<string> purchaseOrder = new List<string>();
                            purchaseOrder.Add(purchaseOrderType);
                            var filters = new Dictionary<string, IEnumerable>
            {
                                  {"ProductTypeCode",(ICollection<string>)productCodes},
                                  {"ProgramCode", (ICollection<string>)progCode},
                                  {"PurchaseOrderTypeCode", (ICollection<string>)purchaseOrder}
            };
                            var cacheKey = productTypeCode + programCode + purchaseOrderType + localeId.ToString();
                            var cacheList = cache.Get(cacheKey);

                            if (cacheList == null && !(filterList.Contains(cacheKey)))
                            {
                                filterList.Add(cacheKey);
                                var ecdmbillingOptions = await GetDomainsAsync<Bedrock_BillingOption>(filters, localeId);
                                var billingOptions = new List<Microsoft.IT.Licensing.Entity.DomainData.BillingOption>();
                                List<BillingOption> cachingList = new List<BillingOption>();
                                foreach (var bedrockbillingOption in ecdmbillingOptions)
                                {
                                    var billingOption = new BillingOption
                                    {
                                        Code = bedrockbillingOption.Code,
                                        Description = bedrockbillingOption.Description,
                                        Id = bedrockbillingOption.Id,
                                        Name = bedrockbillingOption.Name,
                                        BaseBillingOptionCode = bedrockbillingOption.BaseBillingOptionCode,
                                        BillingUnitOfMeasureCode = bedrockbillingOption.BillingUnitOfMeasureCode,
                                        IsChannelPartnerVisible = bedrockbillingOption.IsChannelPartnerVisible,
                                        ProductTypeCode = bedrockbillingOption.ProductTypeCode,
                                        IsValidForUI = bedrockbillingOption.ValidForUI.Trim() == "T" ? true : false,
                                        ProgramCode = bedrockbillingOption.ProgramCode,
                                        PurchaseOrderTypeCode = bedrockbillingOption.PurchaseOrderTypeCode,
                                        PurchaseUnitCode = bedrockbillingOption.PurchaseUnitCode,
                                        BillingMultiplier = (int)bedrockbillingOption.BillingMultiplier
                                    };

                                    output.Add(billingOption);
                                    cachingList.Add(billingOption);
                                }

                                cache.Set(cacheKey, cachingList, 1440);
                            }
                            else if (!(filterList.Contains(cacheKey)))
                            {
                                filterList.Add(cacheKey);
                                foreach (var billingOption in (ICollection<BillingOption>)cacheList)
                                {
                                    output.Add(billingOption);
                                }
                            }
                        }
                    }
                    else
                    {


                        List<string> productCodes = new List<string>();
                        productCodes.Add(productTypeCode);
                        List<string> progCode = new List<string>();
                        progCode.Add(programCode);

                        var filters = new Dictionary<string, IEnumerable>
                                {
                                {"ProductTypeCode",(ICollection<string>)productCodes},
                                  {"ProgramCode", (ICollection<string>)progCode}
                                };
                        var cacheKey = productTypeCode + programCode + localeId.ToString();
                        var cacheList = cache.Get(cacheKey);
                        if (cacheList == null && !(filterList.Contains(cacheKey)))
                        {
                            filterList.Add(cacheKey);
                            var ecdmbillingOptions = await GetDomainsAsync<Bedrock_BillingOption>(filters, localeId);
                            var billingOptions = new List<Microsoft.IT.Licensing.Entity.DomainData.BillingOption>();
                            var cachingList = new List<BillingOption>();
                            foreach (var bedrockbillingOption in ecdmbillingOptions)
                            {
                                var billingOption = new BillingOption
                                {
                                    Code = bedrockbillingOption.Code,
                                    Description = bedrockbillingOption.Description,
                                    Id = bedrockbillingOption.Id,
                                    Name = bedrockbillingOption.Name,
                                    BaseBillingOptionCode = bedrockbillingOption.BaseBillingOptionCode,
                                    BillingUnitOfMeasureCode = bedrockbillingOption.BillingUnitOfMeasureCode,
                                    IsChannelPartnerVisible = bedrockbillingOption.IsChannelPartnerVisible,
                                    ProductTypeCode = bedrockbillingOption.ProductTypeCode,
                                    IsValidForUI = bedrockbillingOption.ValidForUI.Trim() == "T" ? true : false,
                                    ProgramCode = bedrockbillingOption.ProgramCode,
                                    PurchaseOrderTypeCode = bedrockbillingOption.PurchaseOrderTypeCode,
                                    PurchaseUnitCode = bedrockbillingOption.PurchaseUnitCode,
                                    BillingMultiplier = (int)bedrockbillingOption.BillingMultiplier
                                };

                                output.Add(billingOption);
                                cachingList.Add(billingOption);
                            }
                            cache.Set(cacheKey, cachingList, 1440);
                        }
                        else if (!(filterList.Contains(cacheKey)))
                        {
                            filterList.Add(cacheKey);
                            foreach (var billingOption in (ICollection<BillingOption>)cacheList)
                            {
                                output.Add(billingOption);
                            }
                        }

                    }
                }
            }



            return output;
        }

        /// <summary>
        /// Get Billing Option
        /// </summary>
        public async Task<ICollection<Microsoft.IT.Licensing.Entity.DomainData.BillingOption>> GetBillingOptionsAsync(string productTypeCode, string programCode, string purchaseOrderType, int localeId)
        {
            var cacheKey = productTypeCode + programCode + purchaseOrderType + localeId;
            var cacheList = cache.Get(cacheKey);
            if (cacheList == null)
            {
                var billingOptions = await GetBillingOptionsAsync(new List<string> { productTypeCode }, new List<string> { programCode },
                new List<string> { purchaseOrderType }, localeId);

                cache.Set(cacheKey, billingOptions, 1440);
                return billingOptions;

            }
            else
            {
                return (ICollection<Microsoft.IT.Licensing.Entity.DomainData.BillingOption>)cacheList;
            }
        }

        /// <summary>
        /// Get Product Family
        /// </summary>
        public async Task<ICollection<ProductFamily>> GetProductFamilyAsync(ICollection<string> codes, int localeId)
        {
            List<ProductFamily> output = new List<ProductFamily>();
            foreach (var code in codes)
            {

                List<string> programCodes = new List<string>();
                programCodes.Add(code.ToString());

                var filter = new Dictionary<string, IEnumerable>
            {
                {"Code", (ICollection<string>)programCodes}
            };



                if (code == codes.First())
                {
                    var cacheKey = code + localeId;
                    var cacheList = cache.Get(cacheKey);

                    if (cacheList == null)
                    {

                        var products = await GetDomainsAsync<Bedrock_ProductFamily>(filter, null);

                        output = products.Select(domainProduct => new ProductFamily

                        {

                            Code = domainProduct.Code,

                            Description = domainProduct.Description,

                            Id = domainProduct.Id,


                            Name = domainProduct.Name,

                            PoolCode = domainProduct.PoolCode

                        }).ToList();

                        cache.Set(cacheKey, output, 1440);

                    }

                    else
                    {
                        output = (List<ProductFamily>)cacheList;
                    }
                }
                else
                {
                    var cacheKey = code + localeId;
                    var cacheList = cache.Get(cacheKey);

                    if (cacheList == null)
                    {
                        var products = await GetDomainsAsync<Bedrock_ProductFamily>(filter, null);
                        List<ProductFamily> list = products.Select(domainProduct => new ProductFamily

            {

                Code = domainProduct.Code,

                Description = domainProduct.Description,

                Id = domainProduct.Id,


                Name = domainProduct.Name,

                PoolCode = domainProduct.PoolCode

            }).ToList();
                        cache.Set(cacheKey, list, 1440);
                        output = output.Concat(list).ToList();

                    }
                    else
                    {
                        output = output.Concat((List<ProductFamily>)cacheList).ToList();
                    }
                }
            }


            return output;

        }
        /// <summary>
        /// Get Program Offering
        /// </summary>
        public async Task<ICollection<ProgramOffering>> GetProgramOfferingAsync(ICollection<string> codes, int localeId)
        {
            List<ProgramOffering> output = new List<ProgramOffering>();
            foreach (var code in codes)
            {

                List<string> programCodes = new List<string>();
                programCodes.Add(code.ToString());

                var filter = new Dictionary<string, IEnumerable>
            {
                {"Code", (ICollection<string>)programCodes}
            };



                if (code == codes.First())
                {
                    var cacheKey = code + localeId;
                    var cacheList = cache.Get(cacheKey);

                    if (cacheList == null)
                    {

                        var programs = await GetDomainsAsync<Bedrock_ProgramOffering>(filter, localeId);

                        output = programs.Select(domainProgram => new ProgramOffering

                        {

                            Code = domainProgram.Code,

                            Description = domainProgram.Description,

                            Id = domainProgram.Id,

                            Name = domainProgram.Name,

                            EndEffectiveDate = domainProgram.EndEffectiveDate,

                            StartEffectiveDate = domainProgram.StartEffectiveDate,

                            ProgramCode = domainProgram.ProgramCode

                        }).ToList();

                        cache.Set(cacheKey, output, 1440);

                    }

                    else
                    {
                        output = (List<ProgramOffering>)cacheList;
                    }
                }
                else
                {
                    var cacheKey = code + localeId;
                    var cacheList = cache.Get(cacheKey);

                    if (cacheList == null)
                    {
                        var programs = await GetDomainsAsync<Bedrock_ProgramOffering>(filter, localeId);
                        List<ProgramOffering> list = programs.Select(domainProgram => new ProgramOffering

            {
                Code = domainProgram.Code,
                Description = domainProgram.Description,
                Id = domainProgram.Id,
                Name = domainProgram.Name,
                EndEffectiveDate = domainProgram.EndEffectiveDate,
                StartEffectiveDate = domainProgram.StartEffectiveDate,
                ProgramCode = domainProgram.ProgramCode
            }).ToList();
                        cache.Set(cacheKey, list, 1440);
                        output = output.Concat(list).ToList();

                    }
                    else
                    {
                        output = output.Concat((List<ProgramOffering>)cacheList).ToList();
                    }
                }
            }


            return output;
        }

        /// <summary>
        /// Get Domain Country
        /// </summary>
        public async Task<ICollection<Country>> GetDomainCountryAsync(int localeId)
        {

            var domaincountries = cache.Get(localeId.ToString());
            if (domaincountries == null)
            {
                var countries = await GetDomainsAsync<Bedrock_Country>(localeId);
                var cacheData = countries.Select(domainCountry => new Country{
                BillsInCurrencyCode = domainCountry.BillsInCurrencyCode,
                CorrespondenceLanguageCode = domainCountry.CorrespondenceLanguageCode,
                FulfillmentGroupingCode = domainCountry.FulfillmentGroupingCode,
                PostalRuleCode = domainCountry.PostalRuleCode,
                PriceListCountryCode = domainCountry.PriceListCountryCode,
                SalesLocationCode = domainCountry.SalesLocationCode,
                SapOriginCountryCode = domainCountry.SAPOriginCountryCode,
                Code = domainCountry.Code,
                Description = domainCountry.Description,
                Id = domainCountry.Id,
                Name = domainCountry.Name}).ToList();
                IEnumerable<Country> query = from items in cacheData orderby items.Name select items;
                var result = query.ToList();
                cache.Set(localeId.ToString(), result, 1440);
                return result;
            }
            else
            {
                return (ICollection<Country>)domaincountries;
            }


        }

        /// <summary>
        /// Get PSS Status
        /// </summary>

        /// <summary>
        /// Get Locale
        /// </summary>
        public List<Web.UI.Repositories.Models.DomainItem> GetSupportedLanguages()
        {
            var supportedLocale = ConfigurationManager.AppSettings["SupportedLocale"];
            if (null == supportedLocale)
            {
                throw new ArgumentException("Supported Locale setting not defined.");
            }
            string[] locale = supportedLocale.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var filters = new Dictionary<string, IEnumerable> { { "Code", locale.ToList() } };
            var domainEntites = ODataHelper.ODataRestQuery<Bedrock_Locale>(_domainDataEntities, filters);
            var locales = domainEntites.ToList();

            return locales.ConvertAll<Web.UI.Repositories.Models.DomainItem>(bedrockLocale => 
                new Web.UI.Repositories.Models.DomainItem() { Name = bedrockLocale.Name, Code = bedrockLocale.Code, Category=bedrockLocale.ParentLocaleCode});
        }



        public ICollection<PurchaseOrderType> GetPurchaseOrderType()
        {

           List<PurchaseOrderType> PurchaseOrdType= new List<PurchaseOrderType>() ;
            PurchaseOrdType.Add(new PurchaseOrderType{Code="BEC",	Name="Basic Enterprise Commitment"	,EnterpriseRulesFlag="T"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="CRM"	,Name="Credit Memo",	EnterpriseRulesFlag="F"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="EXT",	Name="Term Extension Period"	,EnterpriseRulesFlag="F"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="NE" ,	Name="New Order",	EnterpriseRulesFlag="F"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="RES",	Name="Online Service Reservation",	EnterpriseRulesFlag="F"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="TUP",	Name="True Up",	EnterpriseRulesFlag="T"});
            PurchaseOrdType.Add(new PurchaseOrderType{Code="ZU" ,	Name="Zero Usage",	EnterpriseRulesFlag="F"});
            PurchaseOrdType.Add(new PurchaseOrderType { Code = "NCP", Name = "Change Of Channel Partner PO", EnterpriseRulesFlag = "F" });

            return PurchaseOrdType;
        }

        public ICollection<PurchaseUnit> GetPurchaseUnit()
        {
            List<PurchaseUnit> PurchaseUnit= new List<PurchaseUnit>() ;
            PurchaseUnit.Add(new PurchaseUnit{Code="1M", 	Name="1 Month(s)",	UnitOfMeasureCode="MON",  	Quantity="1",	MSPID="7"});
            PurchaseUnit.Add(new PurchaseUnit{Code="1Q", 	Name="1 Quarter",  	UnitOfMeasureCode="QTR",	Quantity="1",	MSPID="5"});
            PurchaseUnit.Add(new PurchaseUnit{Code="1S", 	Name="Semi-Annual",	UnitOfMeasureCode="QTR",  	Quantity="2",	MSPID="NULL"});
            PurchaseUnit.Add(new PurchaseUnit{Code="1Y", 	Name="1 Years(s)",	UnitOfMeasureCode="YR",   	Quantity="1",	MSPID="3"});
            PurchaseUnit.Add(new PurchaseUnit{Code="2Y", 	Name="2 Year(s)	",   	UnitOfMeasureCode="YR",	 	Quantity="2",   MSPID="4"});    
            PurchaseUnit.Add(new PurchaseUnit{Code="EA", 	Name="Each",   		UnitOfMeasureCode="EA",		Quantity="1",	MSPID="2"});
            PurchaseUnit.Add(new PurchaseUnit { Code = "NON", Name = "Non-specific", UnitOfMeasureCode = "NON", Quantity = "0", MSPID = "1" });
            return PurchaseUnit;
        }



        public ICollection<PurchaseUnitQuantity> GetPurchaseUnitQuantity()
        {
             List<PurchaseUnitQuantity> PurchaseUnitQty= new List<PurchaseUnitQuantity>() ;
             
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="49",Name="12",Code="12",LocalizedPhraseGUID="53477315-F6EC-47F2-B748-922614E3B126",ProgramCode="CC"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="50",Name="Coterminous",Code="-1",LocalizedPhraseGUID="CBDB6B4A-B1DA-48F0-BAAB-4DE42D115FF3",ProgramCode="CC"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="51",Name="12",Code="12",LocalizedPhraseGUID="9B1C6360-77E1-4F8E-86D3-E66711627F6D",ProgramCode="E5"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="52",Name="Coterminous",Code="-1",LocalizedPhraseGUID="882C0F95-5E4B-41BD-BE37-ECAD385A51BB",ProgramCode="E5"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="53",Name="12",Code="12",LocalizedPhraseGUID="18FC3BED-04AA-4A33-95E4-E93E02834C88",ProgramCode="E6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="54",Name="Coterminous",Code="-1",LocalizedPhraseGUID="9460A430-20F2-4198-A761-18C606732B9D",ProgramCode="E6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="55",Name="12",Code="12",LocalizedPhraseGUID="A15E42CD-7050-4042-BFC4-657AE0E31984",ProgramCode="ES"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="56",Name="Coterminous",Code="-1",LocalizedPhraseGUID="2F84E91A-BB98-4A73-9845-31E33F9470A0",ProgramCode="ES"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="57",Name="12",Code="12",LocalizedPhraseGUID="2A2C5ABC-26A6-464E-82FB-27D8C539E9E7",ProgramCode="ESU"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="58",Name="Coterminous",Code="-1",LocalizedPhraseGUID="A76695B7-9272-40BB-AD05-60E7C71DE6FE",ProgramCode="ESU"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="59",Name="12",Code="12",LocalizedPhraseGUID="D8A4CE20-F800-44F7-AE24-7ABEBC70BFA9",ProgramCode="EU"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="60",Name="Coterminous",Code="-1",LocalizedPhraseGUID="0687A92A-24F5-4B18-B9CE-779441432AE7",ProgramCode="EU"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="61",Name="12",Code="12",LocalizedPhraseGUID="3437F1DC-156F-46DD-B50C-E41DCD23D067",ProgramCode="MYO"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="62",Name="Coterminous",Code="-1",LocalizedPhraseGUID="742B5A6C-F236-4715-BAB7-1B461A2069F4",ProgramCode="MYO"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="63",Name="12",Code="12",LocalizedPhraseGUID="B0C34CD3-1ADD-4143-BEAC-A7E5C51D8160",ProgramCode="O6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="64",Name="Coterminous",Code="-1",LocalizedPhraseGUID="D6DC9223-DD57-4D81-BBC6-3E9BA32215C2",ProgramCode="O6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="65",Name="12",Code="12",LocalizedPhraseGUID="EE88C7E4-00C2-4281-A7FC-370F9FBD366F",ProgramCode="OLV"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="66",Name="Coterminous",Code="-1",LocalizedPhraseGUID="CFBFE80F-66D8-4D36-A771-EC16F1679B83",ProgramCode="OLV"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="67",Name="12",Code="12",LocalizedPhraseGUID="CFD035D8-8932-4D5D-B557-A3C0A0CE9B38",ProgramCode="OSL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="68",Name="Coterminous",Code="-1",LocalizedPhraseGUID="078DEF0D-1EBB-4D45-B879-9B053304A4AB",ProgramCode="OSL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="69",Name="12",Code="12",LocalizedPhraseGUID="8ABE3AC9-199C-4BB7-A625-D6F17AD0BD95",ProgramCode="OVS"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="70",Name="Coterminous",Code="-1",LocalizedPhraseGUID="A0A2CC96-36DD-41A0-BD0A-0FF0FB6C94B8",ProgramCode="OVS"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="71",Name="12",Code="12",LocalizedPhraseGUID="731D2F51-C513-4A1E-9D03-E7A9E731F59E",ProgramCode="S4"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="72",Name="Coterminous",Code="-1",LocalizedPhraseGUID="97B39512-72E7-4A3A-BFD5-865B759C679C",ProgramCode="S4"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="73",Name="12",Code="12",LocalizedPhraseGUID="9703AEC5-CDC7-449B-8BDE-EB34947B556A",ProgramCode="S5"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="74",Name="Coterminous",Code="-1",LocalizedPhraseGUID="8A90C9C2-5938-470A-BEBC-6E168CE21E22",ProgramCode="S5"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="75",Name="12",Code="12",LocalizedPhraseGUID="4352E657-D6FB-4240-9CE1-7FF38A205D9D",ProgramCode="S6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="76",Name="Coterminous",Code="-1",LocalizedPhraseGUID="2590353B-8598-45EC-BAB2-CD38D19A6AB9",ProgramCode="S6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="77",Name="12",Code="12",LocalizedPhraseGUID="2F49411C-90C4-4D2F-B544-80FDC80FF7F6",ProgramCode="SC"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="78",Name="Coterminous",Code="-1",LocalizedPhraseGUID="EB054D3F-F5F9-42C2-B720-AED57D95536D",ProgramCode="SC"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="79",Name="12",Code="12",LocalizedPhraseGUID="1EB3F076-415A-481C-A4DC-32C408D248CC",ProgramCode="SEL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="80",Name="Coterminous",Code="-1",LocalizedPhraseGUID="20E52176-E382-4F00-9424-E1A2A0FA8378",ProgramCode="SEL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="81",Name="12",Code="12",LocalizedPhraseGUID="5F637112-6316-467B-ABB7-366817D70884",ProgramCode="SLO"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="82",Name="Coterminous",Code="-1",LocalizedPhraseGUID="D816A169-92A2-4E92-863B-DB41B7FF5AFB",ProgramCode="SLO"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="83",Name="12",Code="12",LocalizedPhraseGUID="FE4BC42E-06D8-4112-8333-D8E1E0B3E6BA",ProgramCode="SLP"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="84",Name="2nd Anniversary",Code="-1",LocalizedPhraseGUID="72D34ECA-2100-4E64-8FBD-221243CFE98F",ProgramCode="SLP"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="85",Name="12",Code="12",LocalizedPhraseGUID="54E8A328-804E-4C35-B19A-FD1A0EA19863",ProgramCode="SMP"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="86",Name="Coterminous",Code="-1",LocalizedPhraseGUID="36942B72-7FAD-4875-94FD-862D633FAFD2",ProgramCode="SMP"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="87",Name="12",Code="12",LocalizedPhraseGUID="33656AAD-FD48-4963-81F4-A29929DD7D83",ProgramCode="USG"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="88",Name="Coterminous",Code="-1",LocalizedPhraseGUID="7267FB7D-EFBC-43A3-8AAD-AC8B120B07DE",ProgramCode="USG"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="89",Name="12",Code="12",LocalizedPhraseGUID="34BBBC05-CCC6-4AFE-8772-7D5810D17ADB",ProgramCode="V6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="90",Name="Coterminous",Code="-1",LocalizedPhraseGUID="6DB58B16-50B9-47D8-A7EA-F084C1582F95",ProgramCode="V6"}); 
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="91",Name="12",Code="12",LocalizedPhraseGUID="68809C7C-0056-47FD-AA30-F06F4079C0D8",ProgramCode="VAL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="92",Name="Coterminous",Code="-1",LocalizedPhraseGUID="A3C06CC0-A7F9-4D12-BBAE-1EAFA5721863",ProgramCode="VAL"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity{PurchaseUnitQuantityID="93",Name="36",Code="36",LocalizedPhraseGUID="5C87081E-6744-49D2-AEF2-422CA964DFB3",ProgramCode="SLP"});
             PurchaseUnitQty.Add(new PurchaseUnitQuantity { PurchaseUnitQuantityID = "95", Name = "Coterminous", Code = "-1", LocalizedPhraseGUID = "0687A92A-24F5-4B18-B9CE-779441432AE7", ProgramCode = "C4" });
             return PurchaseUnitQty;
        }


        public ICollection<RelationshipType> GetRelationShipType()
        {
            List<RelationshipType> RelationShipType = new List<RelationshipType>();
            RelationShipType.Add(new RelationshipType { Name = "Pre-Requisite", Code = "REQ", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Fee with No Coverage", Code = "FNC", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Standard", Code = "STD", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "OLS Renewal", Code = "OSR", IsRenewalFlag = "T", IsStepUpFlag = "F", ROTypeValue = "1" });
            RelationShipType.Add(new RelationshipType { Name = "SA Renewal", Code = "SAR", IsRenewalFlag = "T", IsStepUpFlag = "F", ROTypeValue = "1" });
            RelationShipType.Add(new RelationshipType { Name = "Step-Up", Code = "SUP", IsRenewalFlag = "F", IsStepUpFlag = "T", ROTypeValue = "0" });
            RelationShipType.Add(new RelationshipType { Name = "Renewal", Code = "REN", IsRenewalFlag = "T", IsStepUpFlag = "F", ROTypeValue = "1" });
            RelationShipType.Add(new RelationshipType { Name = "Step-up Non-consumption", Code = "SNC", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Change of Channel Partner", Code = "CCP", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Auto Renew Reject", Code = "ARR", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Overages", Code = "OVG", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "2" });
            RelationShipType.Add(new RelationshipType { Name = "Res Reconciliation", Code = "RRC", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Res Recon Target Arrears", Code = "RTA", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Display Suite Components", Code = "TDC", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Related Transitions", Code = "TRT", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Source to Remainder", Code = "TSR", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Source to New Source", Code = "TSS", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Source to Transition", Code = "TST", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Transition to Target", Code = "TTT", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Unequal Ratio", Code = "TUR", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Reservation to Reservation", Code = "RES", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Restated Source", Code = "TRS", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Res Midterm to Tgt Arrears", Code = "MRA", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Res Midterm to Transition", Code = "MRT", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns MT Prior Src to Trnstn", Code = "MTS", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "True Down", Code = "TDN", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Trns Prior Src to Transition", Code = "TPS", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Extended Term Ended", Code = "ETE", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            RelationShipType.Add(new RelationshipType { Name = "Extended Term Purchase", Code = "ETP", IsRenewalFlag = "F", IsStepUpFlag = "F", ROTypeValue = "" });
            return RelationShipType;
        }

        public ICollection<Currency> GetCurrency()
        {
            List<Currency> Currency= new List<Currency>();
            Currency.Add(new Currency{ Name="Austrian Schilling",Code="ATS",CurrencyID="1",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Australian Dollar",Code="AUD",CurrencyID="2",MasterCurrencyCode="AUD"});
             Currency.Add(new Currency{ Name="Belgian Franc",Code="BEF",CurrencyID="3",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Canadian Dollar",Code="CAD",CurrencyID="4",MasterCurrencyCode="CAD"});
             Currency.Add(new Currency{ Name="Swiss Franc",Code="CHF",CurrencyID="5",MasterCurrencyCode="CHF"});
             Currency.Add(new Currency{ Name="German Mark",Code="DEM",CurrencyID="6",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Danish Krone",Code="DKK",CurrencyID="7",MasterCurrencyCode="DKK"});
             Currency.Add(new Currency{ Name="Euro",Code="EUR",CurrencyID="8",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Spanish Peseta",Code="ESP",CurrencyID="9",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Finland Markka",Code="FIM",CurrencyID="10",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="French Franc",Code="FRF",CurrencyID="11",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Pound Sterling",Code="GBP",CurrencyID="12",MasterCurrencyCode="GBP"});
             Currency.Add(new Currency{ Name="Irish Pound",Code="IEP",CurrencyID="13",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Italian Lira",Code="ITL",CurrencyID="14",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Japanese Yen",Code="JPY",CurrencyID="15",MasterCurrencyCode="JPY"});
             Currency.Add(new Currency{ Name="Netherlands Guilders",Code="NLG",CurrencyID="16",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Norwegian Krone",Code="NOK",CurrencyID="17",MasterCurrencyCode="NOK"});
             Currency.Add(new Currency{ Name="New Zealand Dollar",Code="NZD",CurrencyID="18",MasterCurrencyCode="NZD"});
             Currency.Add(new Currency{ Name="Portuguese Escudo",Code="PTE",CurrencyID="19",MasterCurrencyCode="EUR"});
             Currency.Add(new Currency{ Name="Swedish Krona",Code="SEK",CurrencyID="20",MasterCurrencyCode="SEK"});
             Currency.Add(new Currency{ Name="US Dollar",Code="USD",CurrencyID="21",MasterCurrencyCode="USD"});
             Currency.Add(new Currency{ Name="Yuan Renminbi",Code="CNY",CurrencyID="22",MasterCurrencyCode="CNY"});
             Currency.Add(new Currency{ Name="New Taiwan Dollar",Code="TWD",CurrencyID="23",MasterCurrencyCode="TWD"});
             Currency.Add(new Currency{ Name="Korean Won",Code="KRW",CurrencyID="24",MasterCurrencyCode="KRW"});
             Currency.Add(new Currency{ Name="Thai Baht",Code="THB",CurrencyID="25",MasterCurrencyCode="THB"});
             Currency.Add(new Currency{ Name="Malaysian Ringgit",Code="MYR",CurrencyID="26",MasterCurrencyCode="MYR"});
             Currency.Add(new Currency{ Name="Philippine Peso",Code="PHP",CurrencyID="27",MasterCurrencyCode="PHP"});
             Currency.Add(new Currency{ Name="Indonesian Rupiah",Code="IDR",CurrencyID="29",MasterCurrencyCode="IDR"});
             Currency.Add(new Currency{ Name="Singapore Dollar",Code="SGD",CurrencyID="30",MasterCurrencyCode="SGD"});
             Currency.Add(new Currency{ Name="Argentinian Peso",Code="ARP",CurrencyID="31",MasterCurrencyCode="ARS"});
             Currency.Add(new Currency{ Name="Brazilian Real",Code="BRL",CurrencyID="32",MasterCurrencyCode="BRL"});
             Currency.Add(new Currency{ Name="Chilean Peso",Code="CLP",CurrencyID="33",MasterCurrencyCode="CLP"});
             Currency.Add(new Currency{ Name="Columbian Peso",Code="COP",CurrencyID="34",MasterCurrencyCode="COP"});
             Currency.Add(new Currency{ Name="Mexican Peso",Code="MXP",CurrencyID="35",MasterCurrencyCode="MXN"});
             Currency.Add(new Currency{ Name="Peru Nuevo Sol",Code="PEN",CurrencyID="36",MasterCurrencyCode="PEN"});
             Currency.Add(new Currency{ Name="Uruguay Peso",Code="UYU",CurrencyID="37",MasterCurrencyCode="UYU"});
             Currency.Add(new Currency{ Name="Venezuelan Bolivar",Code="VEB",CurrencyID="38",MasterCurrencyCode="VEB"});
             Currency.Add(new Currency{ Name="South African RAND",Code="ZAR",CurrencyID="41",MasterCurrencyCode="ZAR"});
             Currency.Add(new Currency{ Name="Hong Kong Dollar",Code="HKD",CurrencyID="42",MasterCurrencyCode="HKD"});
             Currency.Add(new Currency{ Name="Russian Ruble",Code="RUB",CurrencyID="43",MasterCurrencyCode="RUB"});
             Currency.Add(new Currency{ Name="Indian Rupee",Code="INR",CurrencyID="44",MasterCurrencyCode="INR"});
             return Currency;
        }

        #endregion

        #region Private Methods

        private async Task<ICollection<T>> GetDomainsAsync<T>(IEnumerable<KeyValuePair<string, IEnumerable>> codes, int? localeId)
        {
            var filters = new Dictionary<string, IEnumerable>();

            if (localeId.HasValue)
            {
                filters.Add("LocaleId", new List<int?> { localeId });
            }

            filters.AddRange(codes);

            var domainEntites = ODataHelper.ODataRestQuery<T>(_domainDataEntities, filters);
            return domainEntites.ToList<T>();
        }

        private async Task<ICollection<T>> GetDomainsAsync<T>(int localeId)
        {
            var filters = new Dictionary<string, IEnumerable> { { "LocaleId", new List<int> { localeId } } };
            var domainEntites = ODataHelper.ODataRestQuery<T>(_domainDataEntities, filters);
            return domainEntites.ToList<T>();
        }

        public static string GetCurrentCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }
        #endregion

        #region Models
        public class PurchaseOrderType : Web.UI.Repositories.Models.DomainItem
        {
            public string EnterpriseRulesFlag { get; set; }
        }
        public class PurchaseUnit : Web.UI.Repositories.Models.DomainItem
        {
            public string UnitOfMeasureCode { get; set; }
            public string Quantity { get; set; }
            public string MSPID { get; set; }

        }
         public class PurchaseUnitQuantity : Web.UI.Repositories.Models.DomainItem
        {
            public string PurchaseUnitQuantityID { get; set; }
            public string LocalizedPhraseGUID { get; set; }
            public string ProgramCode { get; set; }

        }
         public class RelationshipType : Web.UI.Repositories.Models.DomainItem
         {
             public string IsRenewalFlag   { get; set; }
             public string IsStepUpFlag    { get; set; }
             public string ROTypeValue { get; set; }

         }
         public class Currency:Web.UI.Repositories.Models.DomainItem
         {
             public string CurrencyID {get;set;}
             public string MasterCurrencyCode{get;set;}

         }
     
        #endregion
    
    }
}