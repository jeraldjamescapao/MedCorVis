namespace MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;

using System.ComponentModel.DataAnnotations;
using User = MedCorVis.Modules.Identity.Domain.Users.ApplicationUser;

public sealed record ForgotPasswordRequest(
    [Required] [EmailAddress] [MaxLength(User.EmailMaxLength)] string Email);