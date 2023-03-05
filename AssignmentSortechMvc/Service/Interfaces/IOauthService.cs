using AssignmentSortechMvc.Enum;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentSortechMvc.Service.Interfaces
{
    public interface IOauthService
    {
        Task<ValidationResult> GetToken(string code);
        Task<ValidationResult> RefreshToken();
        Task<ValidationResult> RemoveToken();
        Task<string> OauthRedirect();
    }
}
