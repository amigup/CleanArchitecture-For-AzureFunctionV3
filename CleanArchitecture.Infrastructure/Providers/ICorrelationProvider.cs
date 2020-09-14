using System;

namespace AZV3CleanArchitecture.Providers
{
    public interface ICorrelationProvider
    {
        Guid GetCorrelationId();

        Guid? SetCorrelationId();
    }
}