using ASPNETCoreIdentitySample.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ASPNETCoreIdentitySample.Services.Contracts.Identity
{
    public interface IApplicationUserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
    }
}
