using Microsoft.AspNetCore.Identity;

namespace GenmaWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Add address fields
        public string Street { get; set; }
    }
}