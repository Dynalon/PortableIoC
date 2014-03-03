using System;

namespace PortableIoCTests.TestHelpers
{
    public class SimpleFoo : IFoo
    {
        public SimpleFoo()
        {
            UniqueIdentifier = Guid.NewGuid();
        }

        public SimpleFoo(IBar bar) : this()
        {
            Bar = bar;            
        }

        public IBar Bar { get; set; }
        public Guid UniqueIdentifier { get; private set; }
    }
}