using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountingSystemAPI.Helper
{
    public class RoleAuthorizedAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthorizedAttribute(string role)
        {
            _role = role;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("role");
            if (role != _role)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}