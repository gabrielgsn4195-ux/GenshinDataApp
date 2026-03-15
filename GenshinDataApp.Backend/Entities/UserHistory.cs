using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

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

    public string Role { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
