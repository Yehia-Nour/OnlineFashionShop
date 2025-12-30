using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInMin;

        public RedisCacheAttribute(int durationInMin = 5)
        {
            _durationInMin = durationInMin;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cachekey = CreateCacheKey(context.HttpContext.Request);

            var cacheValue = await cacheService.GetAsync(cachekey);
            if (cacheValue is not null)
            {
                context.Result = new ContentResult
                {
                    Content = cacheValue,
                    ContentType = "application/Json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result)
            {
                await cacheService!.SetAsync(cachekey, result.Value!, TimeSpan.FromMinutes(_durationInMin));
            }
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(q => q.Key))
                key.Append($"|{item.Key}-{item.Value}");

            return key.ToString();
        }
    }
}
