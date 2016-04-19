using fit;
using Newtonsoft.Json;
using System;
using System.Linq;
using Web.UI.Repositories;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Test.Fixtures
{
    public abstract class AgreementSearchBaseFixture : ColumnFixture
    {
        protected AgreementSearchBaseFixture()
        {
            AutoMapperConfig.RegisterMappings();
        }
    }

    public class AgreementSearch : AgreementSearchBaseFixture
    {
        public string AgreementNumber { get; set; }
        public string OrgName { get; set; }

        public string OrgPCN { get; set; }

        public bool HasResult()
        {
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();

            //if (!string.IsNullOrEmpty(AgreementNumber))
            agrParams.AgreementNumber = AgreementNumber;

            //if (!string.IsNullOrEmpty(OrgName))
            agrParams.EndCustomerName = OrgName;

            //if (!string.IsNullOrEmpty(OrgPCN))
            agrParams.EndCustomerNumber = OrgPCN;

            agrParams.LookUpDate = dt;
            agrParams.PartnerNumber = null;

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();


            return outputTask.Result.TotalCount > 0;
        }
    }

    public class AgreementSearchByAgrNumber : AgreementSearchBaseFixture
    {
        public string AgreementNumber { get; set; }

        public int MockResult()
        {
            //AutoMapperConfig.RegisterMappings();
            //AgreementRepository agr = new AgreementRepository();
            //DateTime dt = DateTime.Parse("1/1/2014");
            //AgreementRequest agrParams = new AgreementRequest();
            //agrParams.AgreementNumber = AgreementNumber;
            ////agrParams.EndCustomerName = "";
            ////agrParams.EndCustomerNumber = "50523323";
            //agrParams.LookUpDate = dt;
            ////agrParams.SalesLocation = "24";
            //agrParams.PartnerNumber = null;
            ////var output = await agr.SearchOrderableAgreement(agrParams);

            ////Assert.IsTrue(output != null && output.TotalCount == 0);

            ////Console.WriteLine("Records: {0}", output.TotalCount);

            //var outputTask = agr.SearchOrderableAgreement(agrParams);
            //outputTask.Wait();

            return 1; // outputTask.Result.TotalCount;
        }

        public int ServiceResult()
        {           
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.AgreementNumber = AgreementNumber;
            //agrParams.EndCustomerName = "";
            //agrParams.EndCustomerNumber = "50523323";
            agrParams.LookUpDate = dt;
            //agrParams.SalesLocation = "24";
            agrParams.PartnerNumber = null;
            //var output = await agr.SearchOrderableAgreement(agrParams);

            //Assert.IsTrue(output != null && output.TotalCount == 0);

            //Console.WriteLine("Records: {0}", output.TotalCount);

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            return outputTask.Result.TotalCount;
        }
    }


    public class AgreementSearchByOrg : AgreementSearchBaseFixture
    {
        public string OrgName { get; set; }

        public string OrgPCN { get; set; }

        public bool HasResultOf()
        {
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            agrParams.EndCustomerName = OrgName;
            agrParams.LookUpDate = dt;
            agrParams.PartnerNumber = null;

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            var hasResult = outputTask.Result.Results.Any(item => item.EndCustomerNumber != OrgPCN);

            return !hasResult;
        }
    }

    public class AgreementSearchWorkFlow
    {

        private int _resultCount;

        private SearchResult<Agreement> _results;

        public AgreementSearchWorkFlow()
        {
            AutoMapperConfig.RegisterMappings();
        }

        public void SearchAgreements(string agrNum, string orgPCN)
        {
            AgreementRepository agr = new AgreementRepository();
            DateTime dt = DateTime.Parse("1/1/2014");
            AgreementRequest agrParams = new AgreementRequest();
            
            agrParams.AgreementNumber = agrNum;

            ////if (!string.IsNullOrEmpty(OrgName))
            //agrParams.EndCustomerName = OrgName;
            
            agrParams.EndCustomerNumber = orgPCN;

            agrParams.LookUpDate = dt;
            agrParams.PartnerNumber = null;

            var outputTask = agr.SearchOrderableAgreement(agrParams);
            outputTask.Wait();

            _results = outputTask.Result;
            _resultCount = outputTask.Result.TotalCount;
        }

        public int ResultCountIs()
        {
            return _resultCount;
        }

        public string ResponseJSONIs()
        {
            return JsonConvert.SerializeObject(_results);
        }
    }
}
