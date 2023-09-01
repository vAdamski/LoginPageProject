using LoginPageProject.Application.UserActionInfos.Queries.GetUserActionInfos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginPageProject.Controllers;

public class UserActionsController : BaseController
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var vm = await Mediator.Send(new GetUserActionInfosQuery());

        return View(vm);
    }
}