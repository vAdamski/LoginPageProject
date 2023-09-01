using FluentValidation;
using FluentValidation.AspNetCore;
using LoginPageProject.Application.UserAuthentication.Commands.ChangePassword;
using LoginPageProject.Application.UserAuthentication.Commands.LoginUser;
using LoginPageProject.Application.UserAuthentication.Commands.LogoutUser;
using LoginPageProject.Application.UserAuthentication.Commands.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginPageProject.Controllers;

public class UserAuthenticationController : BaseController
{
    private readonly IValidator<RegisterUserCommand> _registerUserCommandValidator;
    private readonly IValidator<LoginUserCommand> _loginUserCommandValidator;
    private readonly IValidator<ChangePasswordCommand> _changePasswordCommandValidator;

    public UserAuthenticationController(
        IValidator<RegisterUserCommand> registerUserCommandValidator,
        IValidator<LoginUserCommand> loginUserCommandValidator,
        IValidator<ChangePasswordCommand> changePasswordCommandValidator)
    {
        _registerUserCommandValidator = registerUserCommandValidator;
        _loginUserCommandValidator = loginUserCommandValidator;
        _changePasswordCommandValidator = changePasswordCommandValidator;
    }
    
    public IActionResult Login()
    {
        if (HttpContext.User.Identity?.Name != null)
            return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    public IActionResult Registration()
    {
        return View();
    }
    
    public IActionResult ChangePassword()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Registration(RegisterUserCommand model)
    {
        ModelState.Clear();
        var validationResult = await _registerUserCommandValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return View("Registration", model);
        }
        
        model.Role = "user";
        var result = await Mediator.Send(model);
        TempData["msg"] = result.Message;
        
        return RedirectToAction("Index", "Home");
    }
    
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await Mediator.Send(new LogoutUserCommand());
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserCommand model)
    {
        ModelState.Clear();
        var validationResult = await _loginUserCommandValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return View("Login", model);
        }
        
        var result = await Mediator.Send(model);
        
        if(result.StatusCode==1)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Login));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult>ChangePassword(ChangePasswordCommand model)
    {
        ModelState.Clear();
        var validationResult = await _changePasswordCommandValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return View("ChangePassword", model);
        }
        
        model.Username = User.Identity.Name;
        var result = await Mediator.Send(model);
        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(ChangePassword));
    }
}