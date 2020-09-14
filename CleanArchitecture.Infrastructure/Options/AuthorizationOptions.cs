using System.ComponentModel.DataAnnotations;

namespace AZV3CleanArchitecture.Options
{
    public class AuthorizationOptions
    {
        [Required]
        public string Authority { get; set; }

        [Required]
        public string CertificateThumbprint { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 36)]
        public string Resource { get; set; }
    }
}
