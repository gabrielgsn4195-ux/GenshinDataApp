using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Language
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NativeName { get; set; } = null!;

    public bool IsDefault { get; set; }

    public virtual ICollection<ArtifactPieceTranslation> ArtifactPieceTranslations { get; set; } = new List<ArtifactPieceTranslation>();

    public virtual ICollection<ArtifactSetTranslation> ArtifactSetTranslations { get; set; } = new List<ArtifactSetTranslation>();

    public virtual ICollection<CharacterConstellationTranslation> CharacterConstellationTranslations { get; set; } = new List<CharacterConstellationTranslation>();

    public virtual ICollection<CharacterTalentTranslation> CharacterTalentTranslations { get; set; } = new List<CharacterTalentTranslation>();

    public virtual ICollection<CharacterTranslation> CharacterTranslations { get; set; } = new List<CharacterTranslation>();

    public virtual ICollection<ElementTranslation> ElementTranslations { get; set; } = new List<ElementTranslation>();

    public virtual ICollection<RegionTranslation> RegionTranslations { get; set; } = new List<RegionTranslation>();

    public virtual ICollection<StatTypeTranslation> StatTypeTranslations { get; set; } = new List<StatTypeTranslation>();

    public virtual ICollection<WeaponRefinementTranslation> WeaponRefinementTranslations { get; set; } = new List<WeaponRefinementTranslation>();

    public virtual ICollection<WeaponTranslation> WeaponTranslations { get; set; } = new List<WeaponTranslation>();

    public virtual ICollection<WeaponTypeTranslation> WeaponTypeTranslations { get; set; } = new List<WeaponTypeTranslation>();
}
