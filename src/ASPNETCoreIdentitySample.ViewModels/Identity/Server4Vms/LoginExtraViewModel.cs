using System.Collections.Generic;

namespace ASPNETCoreIdentitySample.ViewModels.Identity.Server4Vms
{
    public class LoginExtraViewModel : LoginViewModel
    {
        public bool EnableLocalLogin { get; set; }
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
    }

    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}