angular.module('orderApp')
.controller("mockuiCtrl", ['$scope', function ($scope) {

    $scope.savedorders = [
        {
            PONumber: "X651",
            CustomerName: "Fabrikam Research",
            PublicCustomerNumber: "Fabrikam Research",
            AgreementNumber: "6251002",
            DraftName: "FR finish by 2pm today",
            ProgramType: "CUS",
            LastModifiedByDate: "Minerva 9/8/2014",
            Comments: "Eduardo asked that this be done today",
            MicrosoftExtendedAmount: "$8776334.00"
        },
        {
            PONumber: "2 - X651",
            CustomerName: "2 - Fabrikam Research",
            PublicCustomerNumber: "2 - Fabrikam Research",
            AgreementNumber: "2 - 6251002",
            DraftName: "2 - FR finish by 2pm today",
            ProgramType: "2 - CUS",
            LastModifiedByDate: "2 - Minerva 9/8/2014",
            Comments: "2 - Eduardo asked that this be done today",
            MicrosoftExtendedAmount: "2 - $8776334.00"
        },
        {
            PONumber: "2 - X651",
            CustomerName: "2 - Fabrikam Research",
            PublicCustomerNumber: "2 - Fabrikam Research",
            AgreementNumber: "2 - 6251002",
            DraftName: "2 - FR finish by 2pm today",
            ProgramType: "2 - CUS",
            LastModifiedByDate: "2 - Minerva 9/8/2014",
            Comments: "2 - Eduardo asked that this be done today",
            MicrosoftExtendedAmount: "2 - $8776334.00"
        },
        {
            PONumber: "2 - X651",
            CustomerName: "2 - Fabrikam Research",
            PublicCustomerNumber: "2 - Fabrikam Research",
            AgreementNumber: "2 - 6251002",
            DraftName: "2 - FR finish by 2pm today",
            ProgramType: "2 - CUS",
            LastModifiedByDate: "2 - Minerva 9/8/2014",
            Comments: "2 - Eduardo asked that this be done today",
            MicrosoftExtendedAmount: "2 - $8776334.00"
        }
    ];

    $scope.actionrequired = [
       {
           PONumber: "580900",
           CustomerName: "Fabrikam Research",
           PublicCustomerNumber: "98765432",
           AgreementNumber: "4200002400",
           OrderSource: "Batch",
           Type: "New Order",
           PurchaseOrderDate: "9/8/2014",
           CreatedBy: "Sarah",
           PurchaseOrderStatus: "Draft",
           Comments: "Sarah reviewing now - Meryl"
       },
       {
           PONumber: "580900",
           CustomerName: "Fabrikam Research",
           PublicCustomerNumber: "98765432",
           AgreementNumber: "4200002400",
           OrderSource: "New Order",
           Type: "True-Up",
           PurchaseOrderDate: "3/20/2014",
           CreatedBy: "Rajesh",
           PurchaseOrderStatus: "Draft",
           Comments: ""
       },
       {
           PONumber: "580900",
           CustomerName: "Fabrikam Research",
           PublicCustomerNumber: "98765432",
           AgreementNumber: "4200002400",
           OrderSource: "New Order",
           Type: "Bacis Enterprise Commitment",
           PurchaseOrderDate: "3/20/2014",
           CreatedBy: "Rajesh",
           PurchaseOrderStatus: "INvalid",
           Comments: "Rajesh - fix three item numbers Meryl"
       }
    ]

    $scope.ordersdue = [
      {
          PublicCustomerNumber: "98765432",
          CustomerName: "Fabrikam Research",
          AgreementNumber: "4200002400",
          ProgramType: "CUS",
          ContractType: "Government",
          Status: "Active",
          DaysOverdueRemaining: "28"
      },
       {
           PublicCustomerNumber: "98765432",
           CustomerName: "Fabrikam Research",
           AgreementNumber: "4200002400",
           ProgramType: "CUS",
           ContractType: "Government",
           Status: "Active",
           DaysOverdueRemaining: "28"
       }
    ]

    $scope.othercategory = [
     {
         ItemNumber: "BU8-0009",
         OrderingPartName: "Item number not found",
         Quantity: "15",
         PurchaseType: "Transactional",
         PurchaseOption: "Basic",
         LineItemType: "New Order",
         MicrosoftExtendedAmount: "28"
     },
     {
         ItemNumber: "BU8-0009",
         OrderingPartName: "Item number not found",
         Quantity: "15",
         PurchaseType: "Transactional",
         PurchaseOption: "Basic",
         LineItemType: "New Order",
         MicrosoftExtendedAmount: "28"
     }
    ]

    $scope.orderItems = [
     {
         ItemNumber: "btw-00019",
         OrderingPartName: "Win Server Datacenter part 2 Proc SW SA",
         Quantity: "15",
         UsageCountry: ["China", "India"],
         UsageDate: "08/01/2014",
         ProgramOffering: "CUS",
         LineItemType: "New License",
         CoverageDates: "8/22/2014 - 7/31/2015",
         UnitPrice: "2433.35",
         MicrosoftExtendedAmount: "123123.24",
         FlagComments: "Hi",
         IsDisable: false,
         DateFormat: "MM/dd/yyyy"
     }
     ,
      {
          ItemNumber: "btw-00019",
          OrderingPartName: "Win Server Datacenter part 2 Proc SW SA",
          Quantity: "15",
          UsageCountry: ["China", "India"],
          UsageDate: "09/02/2014",
          ProgramOffering: "CUS",
          LineItemType: "New License",
          CoverageDates: "8/22/2014 - 7/31/2015",
          UnitPrice: "2433.35",
          MicrosoftExtendedAmount: "123123.24",
          FlagComments: "Hi",
          IsDisable: true,
          DateFormat: "yyyy/MM/dd"
      },
       {
           ItemNumber: "btw-00019",
           OrderingPartName: "Win Server Datacenter part 2 Proc SW SA",
           Quantity: "15",
           UsageCountry: ["China", "India"],
           UsageDate: "10/03/2014",
           ProgramOffering: "CUS",
           LineItemType: "New License",
           CoverageDates: "8/22/2014 - 7/31/2015",
           UnitPrice: "2433.35",
           MicrosoftExtendedAmount: "123123.24",
           FlagComments: "Hi",
           IsDisable: false,
           DateFormat: "yyyy/MM/dd"
       }
    ]

    $scope.RemoveOrder = function (x, index, ev) {
        x.splice(index, 1);
    }

    $scope.SaveOrdersName = "Saved Orders"; // "Container name"
    $scope.SaveOrdersIsOpen = false; // "true or false"
    $scope.SaveOrdersIsDisable = false;  // "true or false"

    $scope.ActionRequiredName = "Action Required"; // "Container name"
    $scope.ActionRequiredIsOpen = false; // "true or false"
    $scope.ActionRequiredIsDisable = false;  // "true or false"

    $scope.OrdersDueName = "Orders Due"; // "Container name"
    $scope.OrdersDueIsOpen = true; // "true or false"
    $scope.OrdersDueIsDisable = true;  // "true or false"

    $scope.OtherCategoryName = "Other Category"; // "Container name"
    $scope.OtherCategoryIsOpen = false; // "true or false"
    $scope.OtherCategoryIsDisable = true;  // "true or false"

    $scope.OrderInformationName = "Order Information"; // "Container name"
    $scope.OrderInformationIsOpen = true; // "true or false"
    $scope.OrderInformationIsDisable = false;  // "true or false"

    $scope.OrderItemsName = "Order Items"; // "Container name"
    $scope.OrderItemsIsOpen = true; // "true or false"
    $scope.OrderItemsIsDisable = false;  // "true or false"

    $scope.UsageDate = "10/02/2014";
    $scope.UsageDateIsDisable = false;
    $scope.UsageDateFormat = "yyyy/MM/dd";

    $scope.UsageDateFormatSwitch = function () {
        if ($scope.UsageDateFormat == "yyyy/MM/dd") {
            $scope.UsageDateFormat = "MM/dd/yyyy";
        } else {
            $scope.UsageDateFormat = "yyyy/MM/dd";
        }
    }

    $scope.UsageEndDate = "10/05/2014";
    $scope.UsageEndDateIsDisable = false;
    $scope.UsageEndDateFormat = "yyyy/MM/dd";
}]);