using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.ViewModels.MetaData
{
    public class SchoolEntryMetadata 
    {
        [HiddenInput]
        public int SchoolId { get; set; }
        
        [Required]
        [DisplayName("School Name"),MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [DisplayName("District"), MaxLength(64)]
        public string District { get; set; }

        [Required]
        [DisplayName("Address Line 1"), MaxLength(128)]
        public string AddressLine1 { get; set; }

        [Required]
        [DisplayName("Address Line 2"), MaxLength(128)]
        public string AddressLine2 { get; set; }

        [Required]
        [DisplayName("Zip Code"), MaxLength(5)]
        public string ZipCode { get; set; }

        [Required]
        [DisplayName("State")]
        public string State { get; set; }

        [Required]
        [HiddenInput() ]
        public int StateId { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [HiddenInput()]
        public int CityId { get; set; }

        [Required]
        [DisplayName("Business Phone"), Phone]
        public string BusinessPhone { get; set; }

        [Required]
        [DisplayName("Cell Phone"), Phone]
        public string CellPhone { get; set; }

        [Required]
        [DisplayName("Fax"), Phone]
        public string Fax { get; set; }

        [Required]
        [DisplayName("Email"),EmailAddress]
        public string Email { get; set; }

    }
}
