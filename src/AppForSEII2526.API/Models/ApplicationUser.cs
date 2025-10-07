using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    public ApplicationUser()
    {
    }

    public ApplicationUser(string name, string surname, string country, string userName)
    {
        Name = name;
        Surname = surname;
        Country = country;
    }

    public int ApplicationUserId { get; set; }

    //Puede que los Display no hagan falta, pero esten en el proyecto de ejemplo.

    [Display(Name = "Name")]
    public string Name { get; set; }

    [Display(Name = "Surname")]
    public string Surname { get; set; }

    [Display(Name = "Country")]
    public string Country { get; set; }

    // Relación con Review (un ApplicationUser puede tener varios Reviews)
    public IList<Review> Reviews { get; set; } = new List<Review>();
    public IList<Purchase> Purchases { get; set; } = new List<Purchase>();

}