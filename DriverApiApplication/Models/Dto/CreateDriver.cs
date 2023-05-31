using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace DriverApiApplication.Models.Dto
{
    [DataContract]
    public class CreateDriver
    {
        [Required]
        [DataMember]
        public string? FirstName { get; set; }

        [Required]
        [DataMember]
        public string? LastName { get; set; }


        [Required]
        [EmailAddress]
        [DataMember]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [DataMember]
        public string? PhoneNumber { get; set; }
    }
}
