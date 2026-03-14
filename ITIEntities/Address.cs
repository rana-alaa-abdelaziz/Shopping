using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITIEntities
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public virtual App_User App_User { get; set; }
        [ForeignKey(nameof(App_User))]
        public String UserId { get; set; }  

        public String Country { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public String Zip { get; set; }

        public bool isDefault { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
