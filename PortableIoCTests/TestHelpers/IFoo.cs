using System;

namespace PortableIoCTests.TestHelpers
{
    public interface IFoo
    {
        IBar Bar { get; }
        Guid UniqueIdentifier { get; }
    }
}