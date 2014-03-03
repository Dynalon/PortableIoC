using System;

namespace PortableIoCTests.TestHelpers
{
    public class SimpleBar : IBar 
    {
        public SimpleBar()
        {
            UniqueIdentifier = Guid.NewGuid();
        }

        public SimpleBar(Exception exception)
        {
            throw exception;
        }

        public Guid UniqueIdentifier { get; private set; }
    }
}