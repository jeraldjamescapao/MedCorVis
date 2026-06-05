namespace MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;

using System.ComponentModel.DataAnnotations;
using User = MedCorVis.Modules.Identity.Domain.Users.ApplicationUser;

public sealed record ChangePasswordRequest(
    [Required] [MinLength(User.PasswordMinLength)] [MaxLength(User.PasswordMaxLength)]string CurrentPassword,
    [Required] [MinLength(User.PasswordMinLength)] [MaxLength(User.PasswordMaxLength)] string NewPassword);