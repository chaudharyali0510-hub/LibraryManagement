namespace LibraryManagement.Services
{
    public interface IPermissionService
    {
        Task<List<string>> GetUserPermissionsAsync(string userId);
        
        Task<bool> UserHasPermissionAsync(
            string userId,
            string permission);
    }
}