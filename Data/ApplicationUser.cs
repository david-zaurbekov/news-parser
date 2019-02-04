using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class ApplicationUser : IUser<int>
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string IpAddress { get; set; }
    }
}