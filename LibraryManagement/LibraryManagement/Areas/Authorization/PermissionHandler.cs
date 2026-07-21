using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Areas.Authorization
{
    public class PermissionHandler : IAuthorizationHandler
    {
        private readonly IPermissionService _permissionService;

        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is PermissionRequirement permReq)
                {
                    var userId = context.User.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                    if (userId == null)
                        continue;

                    var hasPermission = await _permissionService
                        .UserHasPermissionAsync(userId, permReq.Permission);

                    if (hasPermission)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }
    }
}
