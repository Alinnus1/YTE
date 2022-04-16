using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YTE.BusinessLogic.Implementation.FollowList;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Account;
using YTE.BusinessLogic.Implementation.Token;
using Hangfire;

namespace YTE.WebApp.Jobs
{
    public static class BackgroundJobs
    {
        public static void CreateJobs(IServiceProvider serviceProvider)
        {
            var FollowService = serviceProvider.GetService<FollowListService>();
            var ImageService = serviceProvider.GetService<ImageService>();
            var UserService = serviceProvider.GetService<UserAccountService>();
            var TokenService = serviceProvider.GetService<TokenService>();

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            RecurringJob.AddOrUpdate(nameof(TokenService.DeleteExpiredTokens),
                () => TokenService.DeleteExpiredTokens(),
                Cron.Hourly());

            RecurringJob.AddOrUpdate(nameof(UserService.DeleteUnconfirmedUsers),
                () => UserService.DeleteUnconfirmedUsers(),
                Cron.DayInterval(3));

            RecurringJob.AddOrUpdate(nameof(FollowService.SendFollowingLogicNotifications),
                () => FollowService.SendFollowingLogicNotifications(),
                Cron.Weekly);







            
        }
    }
}
