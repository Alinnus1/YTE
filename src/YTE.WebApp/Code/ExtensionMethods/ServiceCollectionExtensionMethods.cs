using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.Account;
using YTE.BusinessLogic.Implementation.Admin;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Film;
using YTE.BusinessLogic.Implementation.FollowList;
using YTE.BusinessLogic.Implementation.ForbiddenWord;
using YTE.BusinessLogic.Implementation.Gender;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.MailSender;
using YTE.BusinessLogic.Implementation.Manga;
using YTE.BusinessLogic.Implementation.Role;
using YTE.BusinessLogic.Implementation.Token;
using YTE.BusinessLogic.Implementation.UserProfile;
using YTE.BusinessLogic.Implementation.VideoGame;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Code.Base;
using YTE.Common.DTOS;

namespace YTE.Code.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();

            return services;
        }

        public static IServiceCollection AddYTEBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<UserAccountService>();
            services.AddScoped<FilmService>();
            services.AddScoped<ArtObjectService>();
            services.AddScoped<MangaService>();
            services.AddScoped<VideoGameService>();
            services.AddScoped<GenreService>();
            services.AddScoped<AdminService>();
            services.AddScoped<GenderService>();
            services.AddScoped<RoleService>();
            services.AddScoped<ImageService>();
            services.AddScoped<ArtReviewService>();
            services.AddScoped<WatchListService>();
            services.AddScoped<FavoriteListService>();
            services.AddScoped<UserProfileService>();
            services.AddScoped<FollowListService>();
            services.AddScoped<ForbiddenWordService>();
            services.AddScoped<TokenService>();
            services.AddScoped<MailSenderService>();
            return services;
        }

        public static IServiceCollection AddYTECurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor.HttpContext;
                if (httpContext == null)
                {
                    return new CurrentUserDto { IsAuthenticated = false };
                }

                var claims = httpContext.User.Claims;
                var list = new List<string>();
                claims?.Where(c => c.Type == ClaimTypes.Role).ToList().ForEach(a => list.Add(a.Value));

                var userIdClaim = claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
                var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);

                return new CurrentUserDto
                {
                    Id = id,
                    IsAuthenticated = httpContext.User.Identity.IsAuthenticated,
                    UserName = httpContext.User.Identity.Name,
                    Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    Roles = list

                };
            });

            return services;
        }
    }
}
