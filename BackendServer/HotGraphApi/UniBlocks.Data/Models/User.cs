﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isUser { get; set; }
        public bool isAdmin { get; set; }

        public ICollection<BlockUser> BlockUsers { get; set; }

    }
}
