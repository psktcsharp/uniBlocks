using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Block
    {
        public Block()
        {
            BlockUsers = new List<BlockUser>();
            isActive = true;
        }
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public string location { get; set; }
        public bool isActive { get; set; }

        public ICollection<BlockUser> BlockUsers { get; set; }

    }
}
