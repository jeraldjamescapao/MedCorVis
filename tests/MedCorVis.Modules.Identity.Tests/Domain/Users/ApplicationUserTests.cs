namespace MedCorVis.Modules.Identity.Tests.Domain.Users;

using FluentAssertions;
using MedCorVis.Modules.Identity.Domain.Users;
using Xunit;

public sealed class ApplicationUserTests
{
    private static ApplicationUser CreateUser(string? culture = null) =>
        ApplicationUser.Create(
            "jjcapao@softwareengineers.ch",
            ApplicationUser.SelfRegisteredActor,
            culture);

    [Fact]
    public void Delete_ValidArgs_AnonymisesPiiAndSetsFlags()
    {
        var user = CreateUser();

        user.Delete("admin");

        user.IsDeleted.Should().BeTrue();
        user.IsActive.Should().BeFalse();
        user.DeletedAtUtc.Should().NotBeNull();
        user.DeletedBy.Should().Be("admin");
        user.Email.Should().StartWith("deleted_");
        user.UserName.Should().StartWith("deleted_");
        user.NormalizedEmail.Should().StartWith("DELETED_");
        user.PhoneNumber.Should().BeNull();
    }

    [Fact]
    public void UpdatePreferredCulture_SameCulture_DoesNotUpdateModifiedFields()
    {
        var user = CreateUser("fr");
        var firstModified = user.ModifiedAtUtc;

        user.UpdatePreferredCulture("fr", "admin");

        user.ModifiedAtUtc.Should().Be(firstModified);
    }
}