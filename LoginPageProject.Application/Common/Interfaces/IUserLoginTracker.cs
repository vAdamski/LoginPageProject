namespace LoginPageProject.Application.Common.Interfaces;

public interface IUserLoginTracker
{ 
    Task TrackLogin(string userName);
    Task TrackLogout();
}