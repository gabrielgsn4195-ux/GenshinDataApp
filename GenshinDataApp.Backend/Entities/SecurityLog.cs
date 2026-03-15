using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class SecurityLog
{
    public int Id { get; set; }

    public string IpAddress { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string? UserId { get; set; }

    public string? UserEmail { get; set; }

    public bool Success { get; set; }

    public string? FailureReason { get; set; }

    public string? UserAgent { get; set; }

    public string? AdditionalData { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
