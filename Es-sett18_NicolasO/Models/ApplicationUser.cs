using Es_sett18_NicolasO.Models;
using Microsoft.AspNetCore.Identity;

namespace Es_sett18_NicolasO.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Biglietto> Biglietti { get; set; }
    }
}

public class ApplicationRole : IdentityRole { }
