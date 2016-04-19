angular.module('orderApp')
.controller("orderLineItemCtrl", ['$scope', 'orderSvc', 'domainDataSvc', 'opNotificationSvc', function ($scope, orderSvc, domainDataSvc, opNotificationSvc) {
    $scope.globalModels = orderSvc.getModel();
    
    $scope.status = {
        open: false,
    };

    //Init start
    $scope.UsageCountriesPromise = domainDataSvc.UsageCountries().then(function (data) {
        $scope.UsageCountries = data;
    });
    $scope.FlagReasonsPromise = domainDataSvc.FlagReasons().then(function (data) {
        $scope.FlagReasons = data;
    });

    $scope.$watch('globalModels.agreementModel', function () {
        if ($scope.globalModels.orderModel && $scope.globalModels.agreementModel) {
            $scope.status.open = true;
        } else if ($scope.globalModels.agreementModel) {
            $scope.globalModels.lineItems = []; //clr and init
            $scope.status.open = true;
        }
        else {
            $scope.status.open = false;
        }
    }, true);
    //Init end
    $scope.billingOptionsEnabled = function(item) {
        if (item && item.attributes && item.attributes.BillingOptions) {
            if (Object.keys(item.attributes.BillingOptions).length > 0) {
                return true;
            }
        }
        return false;
    };

    $scope.programOfferingsEnabled = function(item) {
        if (item && item.attributes && item.attributes.ProgramOfferings) {
            if (Object.keys(item.attributes.ProgramOfferings).length > 0) {
                return true;
            }
        }
        return false;
    };

    $scope.purchaseUnitQuantitiesEnabled = function (item) {
        if (item && item.attributes && item.attributes.IsOls && item.attributes.PurchaseUnitQuantities) {
            if (Object.keys(item.attributes.PurchaseUnitQuantities).length > 0) {
                return true;
            }
        }
        return false;
    };

    //Catalog start

    $scope.dialogStatus = {
        superCatalogOpen: false,
        itemsPerPage: 50,
        currentPage: 1,
        pageInterval: 10,
        totalCount: 0
    };

    $scope.$watchCollection('[dialogStatus.itemsPerPage]', function () {
        $scope.searchAndUpdateItem($scope.skuSearchItem, true);
    });

    $scope.stepUpLineItems = {
        Title: 'Upgrades',
        searchedTerm: {},
        isOpportunitiy: true,
        panelExpanded: true,
        itemType: 'STP',
        search: true,
        Columns: [
           { name: 'Upgrades', width: 105, itemWidth: 1 },
           { name: 'Existing Item Name', width: 265, itemWidth: 353, field: 'OriginalItemName' },
           { name: 'Available', width: 100, field: 'QuantityAvailable' },
           { name: 'Upgrade To', width: 250, field: 'ItemName' },
           { name: 'Upgrade Item Number', width: '', field: 'PartNumber' }
        ],
        results: [],
        svc: function (request) {
            request.opportunityType = 'StepUp';
            $scope.searchOpportunitystepUpPromise = orderSvc.searchOpportunity(request).then(function (data) {
                $scope.stepUpLineItems.results = data;
            });
        }
    };
    $scope.renewalLineItems = {
        Title: 'Renewals',
        searchedTerm: {},
        itemType: 'RNW',
        panelExpanded: true,
        isOpportunitiy: true,
        search: true,
        Columns: [
            { name: 'Renewal', width: 105, itemWidth: 1 },
            { name: 'Product Name', width: 265, itemWidth: 353, field: 'ItemName' },
            { name: 'Item Number', width: 100, field: 'PartNumber' },
            { name: 'Usage End Date', width: 250, field: 'CoverageEndDate' },
            { name: 'Seats available in renewal', width: '', field: 'QuantityAvailable' }
        ],
        results: [],
        svc : function(request) {
            request.opportunityType = 'Renewal';
            $scope.searchOpportunityrenewalPromise = orderSvc.searchOpportunity(request).then(function(data) {
                $scope.renewalLineItems.results = data;
            });
        }
    };
    $scope.historyLineItems = {
        Title: 'Purchase History',
        searchedTerm: {},
        isOpportunitiy: true,
        panelExpanded: true,
        search: true,
        Columns: [
           { name: 'Order History', width: 105, itemWidth: 1 },
           { name: 'Item Name', width: 265, itemWidth: 353, field: 'ItemName' },
           { name: 'Pool Name', width: 100, field: 'PoolCode' },
           { name: 'Product Family', width: '', field: 'ProductFamilyName' }
        ],
        results: [],
        svc: function (request) {
            $scope.searchOrderHistoryPromise = orderSvc.searchOrderHistory(request.request.AgreementId).then(function (data) {
                $scope.historyLineItems.results = data;
            });
        }
    };
    $scope.trueUps = {
        Title: 'True-ups',
        searchedTerm: {},
        itemType: 'TUP',
        panelExpanded: true,
        isOpportunitiy: true,
        search: true,
        Columns: [
           { name: 'True-Ups', width: 105, itemWidth: 1 },
           { name: 'Item Name', width: 265, itemWidth: 353, field: 'ItemName' },
           { name: 'Pool Name', width: 100, field: 'CoverageStartDate' },
           { name: 'Type', width: 250, field: 'CoverageStartDate' },
           { name: 'Product Group', width: 100, field: 'CoverageStartDate' },
           { name: 'Edition', width: '', field: 'CoverageStartDate' }
        ],
        results: [],
        svc: function (request) {
            request.opportunityType = 'TrueUp';
            $scope.searchOpportunitytrueUpsPromise = orderSvc.searchOpportunity(request).then(function (data) {
                $scope.trueUps.results = data;
            });
        }
    };
    $scope.newProduct = {
        Title: 'New Products',
        searchedTerm: {},
        panelExpanded: true,
        search: true,
        itemType: 'NEW',
        isOpportunitiy: false,
        Columns: [
           { name: 'New Products', width: 105, itemWidth: 1 },
           { name: 'Item Name', width: 265, itemWidth: 353, field: 'ItemName' },
           { name: 'Pool Name', width: 100, field: 'PoolName' },
           { name: 'Item Number', width: 100, field: 'PartNumber' },
           { name: 'Available', width: '', field: 'QuantityAvailable' }
        ],
        results: [],
        svc: function () {
            $scope.searchAndUpdateItem($scope.skuSearchItem, true);
        }
    };
    $scope.categories = [
        $scope.newProduct,
        $scope.stepUpLineItems,
        $scope.renewalLineItems,
        $scope.historyLineItems
        //$scope.trueUps
    ];

    $scope.catalogPaging = function() {
        if ($scope.dialogStatus.currentPage > 0) {
            $scope.searchAndUpdateItem($scope.skuSearchItem, true);
        }
    };

    $scope.skuSearchItem = {};

    $scope.showCategory = function (title, skipSearch) {
        if (!$scope.dialogStatus.superCatalogOpen) {
            $scope.dialogStatus.superCatalogOpen = true;
            $scope.dialogStatus.currentPage = 1;
        }
        angular.forEach($scope.categories, function (item) {
                item.search = title === true ? true : title == item.Title;
            });
        if (!skipSearch) {
            $scope.searchCatelog();
        }
    }

    $scope.toggleAllCategories = function (expand) {
        angular.forEach($scope.categories, function (item) {
            item.panelExpanded = expand;
        });
    }

    $scope.toggleAllCategoriesItems = function (selected) {
        angular.forEach($scope.categories, function (cate) {
            angular.forEach(cate.results, function(item) {
                item.selected = selected;
            });
        });
    }
    $scope.selectCatelogItem = function (event, item, selector) {
        if ((selector && $(event.toElement||event.target).is(selector)) || !selector) {
            item.selected = !item.selected;
        }
    };
    $scope.superCatalogInit = function () {
        $scope.dialogStatus.superCatalogOpen = true;
        $scope.searchCatelog(true);
    };

    $scope.searchCatelog = function (refresh) {
        angular.forEach($scope.categories, function(item) {
            if (item.search && (refresh || !item.searchedTerm.ItemName || item.searchedTerm.ItemName != $scope.skuSearchItem.ItemName || (item.isOpportunitiy && (item.searchedTerm.ProductFamilyName != $scope.skuSearchItem.ProductFamilyName)))) {

                var request = { request: {
                        AgreementNumber: $scope.globalModels.agreementModel.AgreementNumber,
                        AgreementId: $scope.globalModels.agreementModel.AgreementId,
                        POAgreementNumber: $scope.globalModels.agreementModel.POAgreementNumber,
                        PublicCustomerNumber: $scope.globalModels.agreementModel.EndCustomerNumber,
                        LookupDate: item.POLIUsageDate || new Date(),
                        SortColumn: 'LineItemId',
                        ProductSearchString: $scope.skuSearchItem.ItemName
                    }
                };
                item.results = null;
                if (item.isOpportunitiy || ($scope.skuSearchItem.ItemName && $scope.skuSearchItem.ItemName.length > 3)) {
                    $scope.dialogStatus.currentPage = 1;
                    item.searching = true;
                    item.svc(request);
                } else {
                    item.searching = false;
                }
                item.searchedTerm = angular.copy($scope.skuSearchItem);
            }
        });
    };

    $scope.importSku = function () {
        var hasItem = false;
        angular.forEach($scope.categories, function (cate) {
            if (cate.results != null) {
                angular.forEach(cate.results, function (item) {
                    if (item.selected) {
                        item.LineItemType = cate.itemType || 'NEW';
                        item.ExtendedAmount = 0;//(item.UnitPrice * item.PurchaseUnitQuantity.Value * 12);
                        var newItem = angular.copy(item);
                        newItem.ProductFamilyName = null;
                        $scope.searchAndUpdateItem(newItem).then(function (lineItem) {
                            if (lineItem) {
                                $scope.globalModels.lineItems.push(lineItem);
                            }
                        });
                        hasItem = true;
                    }
                });
            }
        });
        if (hasItem) {
            $scope.dialogStatus.superCatalogOpen = false;
            $scope.item.ItemName = '';
            $scope.item.ProductFamilyName = '';
        } else {
            opNotificationSvc.error('No items selected yet');
        }
    };

    //Catelog end

    //coverage date and price calculation
    $scope.getCoverageDateAndEstimate = function (lineItem) {
        if ($scope.globalModels && $scope.globalModels.agreementModel && lineItem && lineItem.POLIUsageDate && $scope.globalModels.orderModel.PurchaseOrderTypeCode) {
            $scope.getCoverageDatePromise = orderSvc.getCoverageDate(
                $scope.globalModels.agreementModel.ProgramCode,
                lineItem.POLIUsageDate,
                $scope.globalModels.agreementModel.ReducedAnniversaryDate,
                $scope.globalModels.agreementModel.EndEffectiveDate,
                $scope.globalModels.agreementModel.ComplianceEnd || new Date(),
                lineItem.PurchaseUnitQuantity,
                lineItem.BillingOptionCode).then(function (data) {
                    lineItem.CoverageStartDate = data.CoverageStartDate;
                    lineItem.CoverageEndDate = data.CoverageEndDate;

                    if (lineItem.ProgramOfferingCode && lineItem.BillingOptionCode && (lineItem.PurchaseUnitQuantity || !lineItem.IsOlsItem) && lineItem.QuantityOrdered > 0) {
                        $scope.getLineItemEstimatePromise = orderSvc.getLineItemEstimate(lineItem, $scope.globalModels.agreementModel, $scope.globalModels.orderModel.PurchaseOrderTypeCode).then(function (result) {
                            lineItem.OfferingLevel = result.OfferingLevel;
                            lineItem.Point = result.Points;
                            lineItem.UnitPrice = result.SystemPrice;
                            lineItem.NetPrice = result.ListPrice;//NetPrice
                            lineItem.ExtendedAmount = (lineItem.UnitPrice * lineItem.QuantityOrdered || 0).toFixed(2);
                        });
                    }
                });
        }
        
    };

    $scope.getSubTotal = function () {
        var total = 0;
        angular.forEach($scope.globalModels.lineItems, function (item) {
            total += ((item.UnitPrice * item.QuantityOrdered) || 0);
        });
        return (total || 0).toFixed(2);
    };

    $scope.getTotalPayment = function () {
        var total = 0;
        angular.forEach($scope.globalModels.lineItems, function (item) {
            total += ((item.NetPrice * item.QuantityOrdered) || 0);
        });
        return (total || 0).toFixed(2);
    };

    $scope.estimatedTax = function () {
        return (($scope.getTotalPayment() - $scope.getSubTotal()) || 0).toFixed(2);
    };

    $scope.getTotalPoints = function () {
        var totalPoints = [], dict = {};
        angular.forEach($scope.globalModels.lineItems, function (item) {
            var pool = item.Pool;
            if ((pool || '') != '') {
                if (!dict[pool]) {
                    var obj = { name: pool, value: item.Point };
                    totalPoints.push(obj);
                    dict[pool] = obj;
                } else {
                    dict[pool].value += item.Point;
                }
            }
        });
        return totalPoints;
    };
    //pricing end

    //line item start
    $scope.item = {
        PartNumber: '',
        panelExpanded: true,
        panelDisabled: true,
        isExample: true
    };

    $scope.deleteItem = function (index) {
        $scope.globalModels.lineItems.splice(index, 1);
        $scope.$broadcast('Re-Register-OpForm');
    };

    $scope.toggleLineItemAll = function (expand) {
        angular.forEach($scope.globalModels.lineItems, function (item) {
            item.panelExpanded = expand;
        });
        $scope.item.panelExpanded = expand;
    };

    $scope.searchAndAddNewItem = function (item) {
        var add = item.isExample;
        if (add) {
            item = {
                PartNumber: $scope.item.PartNumber,
                ItemName: $scope.item.ItemName,
                ProductFamilyName: $scope.item.ProductFamilyName,
                LineItemType: 'NEW'
            };
        }
        var ret = $scope.searchAndUpdateItem(item);
        if (ret && add) {
            ret.then(function (newitem) {
                if (newitem) {
                    $scope.globalModels.lineItems.push(item);
                    $scope.item.PartNumber = '';
                }
            });
        }
    };

    function checkStepUp(lineitem) {
        if (lineitem.IsStepUp && $scope.globalModels.agreementModel.IsAffiliate && (lineitem.itemType == 'NEW' || lineitem.itemType == 'OAP')) { //todo non-roc
            opNotificationSvc.Error('Cannot add stepup opportunity as a new product');
            return false;
        }
        return true;
    }

    $scope.searchAndUpdateItem = function (item, inCatelog) {
        if (item.PartNumber || item.ItemName || item.ProductFamilyName) {
            //var searchedSkuPartNumber = item.PartNumber;
            item.POLIUsageDate = item.POLIUsageDate || new Date();
            return $scope.searchSkuPromise = orderSvc.searchSku(item, $scope.globalModels, $scope.dialogStatus, $scope.dialogStatus.currentPage, $scope.dialogStatus.itemsPerPage).then(function (lineitem) {
                if (lineitem) {
                    if (inCatelog || lineitem.length > 1) {
                        $scope.showCategory("New Products", true);
                        $scope.newProduct.results = lineitem;
                        $scope.skuSearchItem = { ItemName: item.ItemName};
                    } else if (!inCatelog) {
                        //lineitem.searchedSkuPartNumber = searchedSkuPartNumber;
                        lineitem.panelDisabled = false;
                        lineitem.isExample = false;
                        lineitem.panelExpanded = true;
                        if (!checkStepUp(lineitem)) {
                            return null;
                        }
                        $scope.LineItemAttributesPromise = domainDataSvc.LineItemAttributes($scope.globalModels, item);
                        return lineitem;
                    }
                }
                return null;
            });
        }
        return null;
    };

    $scope.cloneItem = function (index) {
        var item = angular.copy($scope.globalModels.lineItems[index]);
        $scope.globalModels.lineItems.splice(index + 1, 0, item);
        $scope.$broadcast('Re-Register-OpForm');
    };

}]);