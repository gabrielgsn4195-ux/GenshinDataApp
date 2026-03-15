using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BuildHistory
{
    public int Id { get; set; }

    public int BuildId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int CharacterId { get; set; }

    public int? WeaponId { get; set; }

    public int? ArtifactFlowerId { get; set; }

    public int? ArtifactPlumeId { get; set; }

    public int? ArtifactSandsId { get; set; }

    public int? ArtifactGobletId { get; set; }

    public int? ArtifactCircletId { get; set; }

    public bool IsShared { get; set; }

    public int Version { get; set; }

    public string? Notes { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
