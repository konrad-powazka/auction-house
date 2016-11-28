using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Web.Models
{
    public class LoginCommand
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}