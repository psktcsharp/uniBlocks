using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    public class AllInput
    {
        public AllInput(string anyString)
        {
            AnyString = anyString;
        }

        public String AnyString { get; }
    }
}
