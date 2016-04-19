Feature: Ordering
	

@searchagreementbyorg
Scenario: Search Agrements by OrgName
	Given I have Portal access
	And I have navigated to Orders Page	
	Then I should be able to search agreements by agrementnumber, customernumber and customername
	When I enter 'Agia' in customer name text box and press Search
	Then I should see only agreements from org pcn '47269393'
	
@searchagreementbyagrnum
Scenario: Search Agrements by AgrNum
	Given I have Portal access
	And I have navigated to Orders Page	
	Then I should be able to search agreements by agrementnumber, customernumber and customername
	When I enter '8070563' in agreement number text box and press Search
	Then I should see '1' agreement


@searchagreementbyagrnum_emptyresults
Scenario: Search Agrements by AgrNum for Empty Results
	Given I have Portal access
	And I have navigated to Orders Page	
	Then I should be able to search agreements by agrementnumber, customernumber and customername
	When I enter '0000000' in agreement number text box and press Search
	Then I should see '0' agreement


@searchagreementbyorgandagrnum
Scenario: Search Agrements by OrgName and AgrNum
	Given I have Portal access
	And I have navigated to Orders Page	
	Then I should be able to search agreements by agrementnumber, customernumber and customername
	When I enter 'Agia' in customer name text box and '8070563' in agreementnumber and press Search	
	Then I should see only agreements from org pcn '47269393'

@searchagreementbyorgandagrnum_datarow
Scenario: Search Agrements by OrgName and AgrNum Data Row
	Given I have Portal access
	And I have navigated to Orders Page	
	Then I should be able to search agreements by agrementnumber, customernumber and customername
	When I search multipe search using customer name agreementnumber 
	| OrgName | AgrNum |
	| Agia  | 8070563  |
	Then I should see agreements
	
	