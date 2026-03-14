using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITIEntities
{
    public class App_User :IdentityUser

    {
        public App_User()
        {
            order = new HashSet<Order>();
            address = new HashSet<Address>();
        }


        public String FullName { get; set; }
        public ICollection<Order> order { get; set; }
        public ICollection<Address> address { get; set; }

    }
}
