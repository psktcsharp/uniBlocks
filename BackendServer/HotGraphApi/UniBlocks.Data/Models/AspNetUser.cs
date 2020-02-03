using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class AspNetUser
    {
        public AspNetUser()
        {
            BlockUsers = new List<BlockUser>();
            UserMessages = new List<UserMessages>();
            Subscriptions = new List<Subscription>();
        }
        public int Id { get; set; }
        [Required]



        public string AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public ICollection<UserMessages> UserMessages { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<BlockUser> BlockUsers { get; set; }
      
  

    }
}
