namespace MedCorVis.Modules.Identity.Tests.Domain.Users;

using FluentAssertions;
using MedCorVis.Common.Exceptions;
using MedCorVis.Modules.Identity.Domain.Users;
using Xunit;

public sealed class ApplicationUserTests
{
    private static ApplicationUser CreateUser(string? culture = null) =>
        ApplicationUser.Create(
            "jjcapao@softwareengineers.ch",
            ApplicationUser.SelfRegisteredActor,
            culture);

    // ─── Create ───────────────────────────────────────────────────────────

    [Fact]
    public void Create_EmptyEmail_ThrowsDomainException()
    {
        var act = () => ApplicationUser.Create(
            string.Empty,
            ApplicationUser.SelfRegisteredActor);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_EmptyCreatedBy_ThrowsDomainException()
    {
        var act = () => ApplicationUser.Create(
            "jjcapao@softwareengineers.ch",
            string.Empty);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_UnsupportedCulture_ThrowsDomainException()
    {
        var act = () => ApplicationUser.Create(
            "jjcapao@softwareengineers.ch",
            ApplicationUser.SelfRegisteredActor,
            "xx-INVALID");

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_ValidArgs_SetsPropertiesCorrectly()
    {
        var user = CreateUser("fr");

        user.Email.Should().Be("jjcapao@softwareengineers.ch");
        user.CreatedBy.Should().Be(ApplicationUser.SelfRegisteredActor);
        user.PreferredCulture.Should().Be("fr");
        user.IsActive.Should().BeTrue();
        user.IsDeleted.Should().BeFalse();
        user.CreatedAtUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    // ─── Delete ───────────────────────────────────────────────────────────

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
    public void Delete_EmptyDeletedBy_ThrowsDomainException()
    {
        var user = CreateUser();

        var act = () => user.Delete(string.Empty);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Delete_AlreadyDeleted_IsIdempotent()
    {
        var user = CreateUser();
        user.Delete("admin");
        var firstDeletedAt = user.DeletedAtUtc;

        user.Delete("admin");

        user.DeletedAtUtc.Should().Be(firstDeletedAt);
    }

    // ─── Activate / Deactivate ────────────────────────────────────────────

    [Fact]
    public void Deactivate_ActiveUser_SetsIsActiveFalse()
    {
        var user = CreateUser();

        user.Deactivate("admin");

        user.IsActive.Should().BeFalse();
        user.ModifiedAtUtc.Should().NotBeNull();
        user.ModifiedBy.Should().Be("admin");
    }

    [Fact]
    public void Deactivate_AlreadyInactive_IsIdempotent()
    {
        var user = CreateUser();
        user.Deactivate("admin");
        var firstModified = user.ModifiedAtUtc;

        user.Deactivate("admin");

        user.ModifiedAtUtc.Should().Be(firstModified);
    }

    [Fact]
    public void Activate_InactiveUser_SetsIsActiveTrue()
    {
        var user = CreateUser();
        user.Deactivate("admin");

        user.Activate("admin");

        user.IsActive.Should().BeTrue();
        user.ModifiedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void Activate_AlreadyActive_IsIdempotent()
    {
        var user = CreateUser();
        var firstModified = user.ModifiedAtUtc;

        user.Activate("admin");

        user.ModifiedAtUtc.Should().Be(firstModified);
    }

    // ─── RequestDeletion / CancelDeletionRequest ──────────────────────────

    [Fact]
    public void RequestDeletion_ValidUser_SetsDeletionRequestedAtUtc()
    {
        var user = CreateUser();

        user.RequestDeletion();

        user.DeletionRequestedAtUtc.Should().NotBeNull();
        user.DeletionRequestedAtUtc.Should()
            .BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void RequestDeletion_AlreadyPending_IsIdempotent()
    {
        var user = CreateUser();
        user.RequestDeletion();
        var firstRequestedAt = user.DeletionRequestedAtUtc;

        user.RequestDeletion();

        user.DeletionRequestedAtUtc.Should().Be(firstRequestedAt);
    }

    [Fact]
    public void RequestDeletion_AlreadyDeleted_IsIdempotent()
    {
        var user = CreateUser();
        user.Delete("admin");

        user.RequestDeletion();

        user.DeletionRequestedAtUtc.Should().BeNull();
    }

    [Fact]
    public void CancelDeletionRequest_PendingRequest_ClearsDeletionRequestedAtUtc()
    {
        var user = CreateUser();
        user.RequestDeletion();

        user.CancelDeletionRequest();

        user.DeletionRequestedAtUtc.Should().BeNull();
    }

    [Fact]
    public void CancelDeletionRequest_NoPendingRequest_IsIdempotent()
    {
        var user = CreateUser();

        var act = () => user.CancelDeletionRequest();

        act.Should().NotThrow();
        user.DeletionRequestedAtUtc.Should().BeNull();
    }

    // ─── UpdatePreferredCulture ───────────────────────────────────────────

    [Fact]
    public void UpdatePreferredCulture_SameCulture_DoesNotUpdateModifiedFields()
    {
        var user = CreateUser("fr");
        var firstModified = user.ModifiedAtUtc;

        user.UpdatePreferredCulture("fr", "admin");

        user.ModifiedAtUtc.Should().Be(firstModified);
    }

    [Fact]
    public void UpdatePreferredCulture_DifferentCulture_UpdatesPreferredCultureAndModifiedFields()
    {
        var user = CreateUser("fr");

        user.UpdatePreferredCulture("de", "admin");

        user.PreferredCulture.Should().Be("de");
        user.ModifiedAtUtc.Should().NotBeNull();
        user.ModifiedBy.Should().Be("admin");
    }
}