using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    [ExtendObjectType(Name = "Mutation")]
    public class AllMutations : IDisposable
    {
        public async Task<AllPayloads> CreateHello(
            AllInput input,
            [Service]IDataRepository dataRepo)
        {
            var newString = input.AnyString;
            dataRepo.AddMsg(new Models.MsgTest() { storedMsg = newString });
            return new AllPayloads(newString);
        }
        public async Task<AllServices> ReadServices(
             [Service]IDataRepository dataRepo, [Service]UniBlocksDBContext uniBlocks
            )
        {
            return new AllServices(dataRepo.GetAllServices(uniBlocks));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AllMutations()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
