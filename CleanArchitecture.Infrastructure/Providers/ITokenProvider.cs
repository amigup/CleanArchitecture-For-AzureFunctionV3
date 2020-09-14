using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Providers
{
    public interface ITokenProvider
    {
        Task<string> GenerateAppOnlyAccessTokenAsync(Guid tenantId);

        Task<string> GenerateAppPlusUserAccessTokenAsync(Guid tenantId);
    }
}
