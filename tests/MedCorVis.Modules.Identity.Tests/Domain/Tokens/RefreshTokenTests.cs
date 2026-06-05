namespace MedCorVis.Modules.Identity.Tests.Domain.Tokens;

using FluentAssertions;
using MedCorVis.Common.Exceptions;
using MedCorVis.Modules.Identity.Domain.Tokens;
using Xunit;

public sealed class RefreshTokenTests
{
    private static RefreshToken CreateValid(
        string? ipAddress = null,
        string? userAgent = null) =>
            RefreshToken.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "hashed-token",
                DateTimeOffset.UtcNow.AddDays(7),
                ipAddress,
                userAgent);

    // ─── Create ───────────────────────────────────────────────────────────

    [Fact]
    public void Create_EmptyUserId_ThrowsDomainException()
    {
        var act = () => RefreshToken.Create(
            Guid.Empty,
            Guid.NewGuid(),
            "token",
            DateTimeOffset.UtcNow.AddDays(1));

        act.Should().Throw<DomainException>()
            .WithMessage("*UserId*");
    }

    [Fact]
    public void Create_EmptyFamilyId_ThrowsDomainException()
    {
        var act = () => RefreshToken.Create(
            Guid.NewGuid(),
            Guid.Empty,
            "token",
            DateTimeOffset.UtcNow.AddDays(1));

        act.Should().Throw<DomainException>()
            .WithMessage("*FamilyId*");
    }

    [Fact]
    public void Create_EmptyToken_ThrowsDomainException()
    {
        var act = () => RefreshToken.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            string.Empty,
            DateTimeOffset.UtcNow.AddDays(1));

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_PastExpiry_ThrowsDomainException()
    {
        var act = () => RefreshToken.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "token",
            DateTimeOffset.UtcNow.AddDays(-1));

        act.Should().Throw<DomainException>()
            .WithMessage("*future*");
    }

    [Fact]
    public void Create_ValidArgs_SetsPropertiesCorrectly()
    {
        var userId   = Guid.NewGuid();
        var familyId = Guid.NewGuid();

        var token = RefreshToken.Create(
            userId, familyId, "hashed-token", DateTimeOffset.UtcNow.AddDays(7));

        token.UserId.Should().Be(userId);
        token.FamilyId.Should().Be(familyId);
        token.Token.Should().Be("hashed-token");
        token.IsRevoked.Should().BeFalse();
        token.IsActive.Should().BeTrue();
        token.RevokedAtUtc.Should().BeNull();
        token.ReplacedByTokenId.Should().BeNull();
        token.CreatedAtUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_IpAddressExceedsMaxLength_IsTruncated()
    {
        var longIp = new string('1', RefreshToken.IpAddressMaxLength + 50);

        var token = CreateValid(ipAddress: longIp);

        token.IpAddress.Should().HaveLength(RefreshToken.IpAddressMaxLength);
    }

    [Fact]
    public void Create_UserAgentExceedsMaxLength_IsTruncated()
    {
        var longAgent = new string('A', RefreshToken.UserAgentMaxLength + 100);

        var token = CreateValid(userAgent: longAgent);

        token.UserAgent.Should().HaveLength(RefreshToken.UserAgentMaxLength);
    }

    [Fact]
    public void Create_NullIpAndUserAgent_StoresNull()
    {
        var token = CreateValid();

        token.IpAddress.Should().BeNull();
        token.UserAgent.Should().BeNull();
    }

    [Fact]
    public void Create_ShortIpAndUserAgent_StoredAsIs()
    {
        var token = CreateValid(ipAddress: "192.168.1.1", userAgent: "Mozilla/5.0");

        token.IpAddress.Should().Be("192.168.1.1");
        token.UserAgent.Should().Be("Mozilla/5.0");
    }

    // ─── IsExpired / IsActive ─────────────────────────────────────────────

    [Fact]
    public void IsActive_FutureExpiry_ReturnsTrue()
    {
        var token = CreateValid();

        token.IsExpired.Should().BeFalse();
        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActive_RevokedToken_ReturnsFalse()
    {
        var token = CreateValid();
        token.Revoke();

        token.IsActive.Should().BeFalse();
    }

    // ─── Revoke ───────────────────────────────────────────────────────────

    [Fact]
    public void Revoke_SetsIsRevokedAndRevokedAtUtc()
    {
        var token = CreateValid();

        token.Revoke();

        token.IsRevoked.Should().BeTrue();
        token.RevokedAtUtc.Should().NotBeNull();
        token.RevokedAtUtc!.Value
            .Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Revoke_CalledTwice_IsIdempotent()
    {
        var token = CreateValid();
        token.Revoke();
        var firstRevokedAt = token.RevokedAtUtc;

        token.Revoke();

        token.IsRevoked.Should().BeTrue();
        token.RevokedAtUtc.Should().Be(firstRevokedAt);
    }

    // ─── MarkReplacedBy ───────────────────────────────────────────────────

    [Fact]
    public void MarkReplacedBy_EmptyGuid_ThrowsDomainException()
    {
        var token = CreateValid();

        var act = () => token.MarkReplacedBy(Guid.Empty);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkReplacedBy_ValidGuid_SetsReplacedByTokenId()
    {
        var token      = CreateValid();
        var newTokenId = Guid.NewGuid();

        token.MarkReplacedBy(newTokenId);

        token.ReplacedByTokenId.Should().Be(newTokenId);
    }
}