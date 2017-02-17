using System.Threading.Tasks;
using ASPNETCoreIdentitySample.Services.Contracts.Identity;
using ASPNETCoreIdentitySample.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ResourceWithIdentityServerWithClient.ViewComponents
{
    public class OnlineUsersViewComponent : ViewComponent
    {
        private readonly ISiteStatService _siteStatService;

        public OnlineUsersViewComponent(ISiteStatService siteStatService)
        {
            _siteStatService = siteStatService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int numbersToTake, int minutesToTake, bool showMoreItemsLink)
        {
            var usersList = await _siteStatService.GetOnlineUsersListAsync(numbersToTake, minutesToTake).ConfigureAwait(false);
            return View(viewName: "~/Views/Shared/Components/OnlineUsers/Default.cshtml",
                        model: new OnlineUsersViewModel
                        {
                            MinutesToTake = minutesToTake,
                            NumbersToTake = numbersToTake,
                            ShowMoreItemsLink = showMoreItemsLink,
                            Users = usersList
                        });
        }
    }
}