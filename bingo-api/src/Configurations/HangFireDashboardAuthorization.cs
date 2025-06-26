using Hangfire.Dashboard.BasicAuthorization;
namespace bingo_api.src.Configurations;

public class HangFireDashboardAuthorization
{
    public static BasicAuthAuthorizationFilter[] AuthenticationFilters(IConfiguration configuration)
    {
        var username = configuration["HangfireDashboard:Username"];
        var password = configuration["HangfireDashboard:Password"];
        return [new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
        {
            SslRedirect = false,
            RequireSsl = false,
            LoginCaseSensitive = true,
            Users = new[] {
                new BasicAuthAuthorizationUser{
                    Login= username,
                    PasswordClear = password
                }
            }
        })];
    }
}
