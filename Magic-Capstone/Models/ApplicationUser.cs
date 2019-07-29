using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Magic_Capstone.Models;
using Microsoft.AspNetCore.Identity;

namespace Magic_Capstone.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        [Display(Name = "User Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public string StreetAddress { get; set; }

        public virtual ICollection<Card> Products { get; set; }

        public virtual ICollection<Deck> Orders { get; set; }

        
    }
}