namespace MedCore.Modules.Users.Application.Contracts.Requests;

using MedCore.Common.Validations;
using System.ComponentModel.DataAnnotations;

public sealed record UpdateProfileRequest(
    [Required] [MinLength(2)] [MaxLength(100)] string FirstName,
    [Required] [MinLength(2)] [MaxLength(100)] string LastName,
    [PastDate] DateOnly BirthDate);