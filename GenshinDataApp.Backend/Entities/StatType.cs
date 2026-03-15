using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class StatType
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<StatTypeTranslation> StatTypeTranslations { get; set; } = new List<StatTypeTranslation>();

    public virtual ICollection<UserArtifact> UserArtifactMainStats { get; set; } = new List<UserArtifact>();

    public virtual ICollection<UserArtifact> UserArtifactSubStat1s { get; set; } = new List<UserArtifact>();

    public virtual ICollection<UserArtifact> UserArtifactSubStat2s { get; set; } = new List<UserArtifact>();

    public virtual ICollection<UserArtifact> UserArtifactSubStat3s { get; set; } = new List<UserArtifact>();

    public virtual ICollection<UserArtifact> UserArtifactSubStat4s { get; set; } = new List<UserArtifact>();

    public virtual ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
}
