using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class RefreshTokenHistory
{
    public int Id { get; set; }

    public int RefreshTokenId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
