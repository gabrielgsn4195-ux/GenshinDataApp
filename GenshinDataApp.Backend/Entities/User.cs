using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class User
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string Username { get; set; } = null!;

    public string UserCode { get; set; } = null!;

    public string AuthProvider { get; set; } = null!;

    public string? AuthProviderId { get; set; }

    public string? AvatarPath { get; set; }

    public bool IsEmailVerified { get; set; }

    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailVerificationTokenExpiry { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetTokenExpiry { get; set; }

    public DateTime? UsernameLastChangedAt { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<BugReport> BugReportAssignedToUsers { get; set; } = new List<BugReport>();

    public virtual ICollection<BugReport> BugReportUsers { get; set; } = new List<BugReport>();

    public virtual ICollection<Build> Builds { get; set; } = new List<Build>();

    public virtual ICollection<ImportLog> ImportLogs { get; set; } = new List<ImportLog>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<TechnicalPermission> TechnicalPermissionGrantedByUsers { get; set; } = new List<TechnicalPermission>();

    public virtual ICollection<TechnicalPermission> TechnicalPermissionUsers { get; set; } = new List<TechnicalPermission>();

    public virtual ICollection<UserArtifact> UserArtifacts { get; set; } = new List<UserArtifact>();

    public virtual ICollection<UserCharacter> UserCharacters { get; set; } = new List<UserCharacter>();

    public virtual ICollection<UserWeapon> UserWeapons { get; set; } = new List<UserWeapon>();
}
