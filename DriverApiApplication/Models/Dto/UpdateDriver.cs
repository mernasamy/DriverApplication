using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.Serialization;

namespace DriverApiApplication.Models.Dto
{
    [DataContract]
    public class UpdateDriver
    {
        [DataMember]
        public string? FirstName { get; set; }
        [DataMember]
        public string? LastName { get; set; }

        [EmailAddress]
        [DataMember]
        public string? Email { get; set; }

        [Phone]
        [DataMember]
        public string? PhoneNumber { get; set; }
    }
}
