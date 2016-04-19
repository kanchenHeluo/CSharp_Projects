
IF NOT EXISTS(SELECT * FROM MetaWorkflow)
BEGIN
INSERT INTO MetaWorkflow (TypeId, TypeName, TypeDescription) values (0, 'Inner', 'Inner Workflow: Connector');
INSERT INTO MetaWorkflow (TypeId, TypeName, TypeDescription) values (1, 'Outter', 'Outter Workflow: Data Src');
END
GO

IF NOT EXISTS(SELECT * FROM MetaEntity)
BEGIN
INSERT INTO MetaEntity 
(EntityName, DBPrimaryKey)
values ('Contact', 'ContactId');
END
/* UAT:
delete from MetaApp
INSERT INTO MetaApp 
(AppName, Entity, SubscriptionId, ResourceGroupName, WorkflowName, WorkflowTypeId, ActiveStatus, CrmResourceUrl, CrmSecondKey)
values 
('COSMIC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4COSMIC', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('COSMIC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4COSMIC', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('GoTools', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4GoTools', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('GoTools', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4GoTools', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('LCAOMC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAOMC', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('LCAOMC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAOMC', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MPC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MPC', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MPC', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MPC', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('OCTO', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4OCTO', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('OCTO', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4OCTO', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('OnePayRoll', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4OnePayRoll', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('OnePayRoll', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4OnePayRoll', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('SurfaceDP', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4SurfaceDP', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('SurfaceDP', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4SurfaceDP', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSBizDev', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSBizDev', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MSBizDev', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSBizDev', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSCOM', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSCOM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MSCOM', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSCOM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSTravel', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSTravel', 'LogicApp4CRMConnectorContact', 0, 1, '', 'mst_EmployeeId'),
('MSTravel', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSTravel', 'LogicApp4Contact', 1, 1, '', 'mst_EmployeeId'),
('SalesDesk', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4SalesDesk', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('SalesDesk', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4SalesDesk', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('XPSStudio', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4XPSStudio', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('XPSStudio', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4XPSStudio', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('CNECRM', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4CNECRM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('CNECRM', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4CNECRM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('ITShowcase', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4ITShowcase', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('ITShowcase', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4ITShowcase', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('LCAWorldwideSalesGroup', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAWorldwideSalesGroup', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('LCAWorldwideSalesGroup', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAWorldwideSalesGroup', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('AccountingServices', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4AccountingServices', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('AccountingServices', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4AccountingServices', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EPRS', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('EPRS', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EPRS', 'Account', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorAccount', 0, 1, '', 'EmployeeId'),
('EPRS', 'Account', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4Account', 1, 1, '', 'EmployeeId'),
('EPRS', 'IO', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorIO', 0, 1, '', 'EmployeeId'),
('EPRS', 'IO', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4IO', 1, 1, '', 'EmployeeId'),
('WWLP', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4WWLP', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('WWLP', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4WWLP', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EMSCRMUAT', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EMSCRMUAT', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('EMSCRMUAT', 'Contact', Cast ('0e2d2609-d3d7-4fb0-8561-398d4f308021' AS UNIQUEIDENTIFIER), 'ResourceGroup4EMSCRMUAT', 'LogicApp4Contact', 1, 1, '', 'EmployeeId')


*/

/* Prod:
INSERT INTO MetaApp 
(AppName, Entity, SubscriptionId, ResourceGroupName, WorkflowName, WorkflowTypeId, ActiveStatus, CrmResourceUrl, CrmSecondKey)
values 
('GoTools', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4GoTools', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('GoTools', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4GoTools', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('LCAOMC', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAOMC', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('LCAOMC', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAOMC', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MPC', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MPC', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MPC', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MPC', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('OCTO', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4OCTO', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('OCTO', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4OCTO', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('OnePayRoll', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4OnePayRoll', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('OnePayRoll', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4OnePayRoll', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('SurfaceDP', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4SurfaceDP', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('SurfaceDP', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4SurfaceDP', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSBizDev', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSBizDev', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MSBizDev', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSBizDev', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSCOM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSCOM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('MSCOM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSCOM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('MSTravel', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSTravel', 'LogicApp4CRMConnectorContact', 0, 1, '', 'mst_EmployeeId'),
('MSTravel', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4MSTravel', 'LogicApp4Contact', 1, 1, '', 'mst_EmployeeId'),
('SalesDesk', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4SalesDesk', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('SalesDesk', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4SalesDesk', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('XPSStudio', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4XPSStudio', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('XPSStudio', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4XPSStudio', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('CNECRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4CNECRM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('CNECRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4CNECRM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('ITShowcase', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4ITShowcase', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('ITShowcase', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4ITShowcase', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('LCAWorldwideSalesGroup', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAWorldwideSalesGroup', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('LCAWorldwideSalesGroup', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4LCAWorldwideSalesGroup', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('AccountingServices', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4AccountingServices', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('AccountingServices', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4AccountingServices', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EPRS', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('EPRS', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EPRS', 'Account', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorAccount', 0, 1, '', 'EmployeeId'),
('EPRS', 'Account', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4Account', 1, 1, '', 'EmployeeId'),
('EPRS', 'IO', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4CRMConnectorIO', 0, 1, '', 'EmployeeId'),
('EPRS', 'IO', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EPRS', 'LogicApp4IO', 1, 1, '', 'EmployeeId'),
('WWLP', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4WWLP', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('WWLP', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4WWLP', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('EMSCRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EMSCRM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('EMSCRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4EMSCRM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId'),
('CRMRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4CRMRM', 'LogicApp4CRMConnectorContact', 0, 1, '', 'EmployeeId'),
('CRMRM', 'Contact', Cast ('2a44f69c-ec5b-4900-ae98-386d0335202b' AS UNIQUEIDENTIFIER), 'ResourceGroup4CRMRM', 'LogicApp4Contact', 1, 1, '', 'EmployeeId')
*/