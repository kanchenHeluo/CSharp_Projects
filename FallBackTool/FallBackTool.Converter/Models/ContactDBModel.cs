using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Converter.Models
{
    public class ContactDBModel : IDBModel
    {
        #region Properties
        //on-prem
        public string ActionCode {get; set;}

        //DB SRC Field Name
        public int ContactId {get; set;}
        public Guid ContactGuid {get; set;}        
        [Required]
        public long personnelnumber {get; set;}
        public string firstname {get; set;}
        public string lastname {get; set;}
        public string fullname {get; set;}
        public string middlename {get; set;}
        public string emailaddress1 {get; set;}
        public string telephone1 {get; set;}
        public string mobilephone {get; set;}
        public string jobtitle {get; set;}
        public string address1_line1 {get; set;}
        public string address1_line2 {get; set;}
        public string address1_line3 {get; set;}
        public string address1_city {get; set;}
        public string address1_county {get; set;}
        public string address1_postalcode {get; set;}
        public string address1_country {get; set;}
        public string costcenter {get; set;}
        public int costcenternumber {get; set;}
        public string costcentercode {get; set;}
        public string ms_alias {get; set;}
        public string domain {get; set;}
        public string domain_alias {get; set;}
        public StatusCode StatusCode {get; set;}
        public StateCode StateCode {get; set;}
        public DateTime Created {get; set;}
        public DateTime LastModified {get; set;}
        public DateTime StatusLastModified {get; set;}
        public DateTime StateLastModified {get; set;}
        public string PositionNbr {get; set;}
        public string ReportsToPositionNbr {get; set;}
        public string secondaryEmailAddress {get; set;}
        public string workingPositionCountry {get; set;}
        public string RoomNumber {get; set;}
        public string BuildingName {get; set;}
        public string CompanyCode {get; set;}
        public string CompanyName {get; set;}
        public string ManagerFullName {get; set;}
        public string OfficeLocation {get; set;}
        public DateTime FirstRegularHireDate {get; set;}
        public DateTime FirstMsjvHireDate {get; set;}
        public long ManagerPersonnelNumber {get; set;}
        public Guid Manager { get; set; }
        #endregion
    }
}
