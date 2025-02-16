using Hangfire.Dashboard.BasicAuthorization;
namespace bingo_api.src.Configurations;

public class HangFireDashboardAuthorization
{
    public static BasicAuthAuthorizationFilter[] AuthenticationFilters(){
        return [new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions{
            SslRedirect = false,
            RequireSsl = false,
            LoginCaseSensitive = true,
            Users = new[] { 
                new BasicAuthAuthorizationUser{
                    Login= "Admin",
                    PasswordClear = "admin"
                }
            }
        })];
    }
}
