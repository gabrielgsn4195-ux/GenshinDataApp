using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Build
{
    public int Id { get; set; }

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

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual UserArtifact? ArtifactCirclet { get; set; }

    public virtual UserArtifact? ArtifactFlower { get; set; }

    public virtual UserArtifact? ArtifactGoblet { get; set; }

    public virtual UserArtifact? ArtifactPlume { get; set; }

    public virtual UserArtifact? ArtifactSands { get; set; }

    public virtual ICollection<BuildSnapshot> BuildSnapshots { get; set; } = new List<BuildSnapshot>();

    public virtual Character Character { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual UserWeapon? Weapon { get; set; }
}
