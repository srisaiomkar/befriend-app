using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data.IRepositories;
using API.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            /* 
            Logic before the action is executed comes here before calling next()
            */

            var resultContext = await next();

            /* 
            Logic after the action is executed comes here after calling next()
            */
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAllChangesAsync();

        }
    }
}