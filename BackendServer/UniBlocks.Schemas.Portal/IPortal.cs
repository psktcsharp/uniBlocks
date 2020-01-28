using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
    public interface IPortal
    {
        //fake data storage 
        ConcurrentStack<AService> AllServices { get; }
        AService AddAService(AService aservice);
        IObservable<AService> Services(Subscription subscription);

        public class Portal : IPortal
        {

            private readonly ISubject<AService> _servicesStream = new ReplaySubject<AService>(1);
            public ConcurrentStack<AService> AllServices { get; }
            public Portal()
            {
                AllServices = new ConcurrentStack<AService>();
            }
          

            public AService AddAService(AService aservice)
            {
                return AddAService(new AService
                {
                    Name = aservice.Name,
                    AServiceId = aservice.AServiceId
                });
                throw new NotImplementedException();
            }

            public IObservable<AService> Services(Subscription subscription)
            {
                return _servicesStream
                    .Select(aservice =>
                    {               
                        return aservice;
                    })
                    .AsObservable();
            }
            public void AddError(Exception exception)
            {
                _servicesStream.OnError(exception);
            }
        }
    }
}
