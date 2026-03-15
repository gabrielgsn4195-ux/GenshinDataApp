using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class TechnicalPermissionHistory
{
    public int Id { get; set; }

    public int PermissionId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public string Permission { get; set; } = null!;

    public int GrantedByUserId { get; set; }

    public DateTime GrantedAt { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
