using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Character
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? Title { get; set; }

    public int ElementId { get; set; }

    public int Rarity { get; set; }

    public int RegionId { get; set; }

    public int WeaponTypeId { get; set; }

    public string? Description { get; set; }

    public int? AscensionStatId { get; set; }

    public string? ImagePath { get; set; }

    public string? ThumbnailPath { get; set; }

    public string? IconPath { get; set; }

    public decimal? BaseHp { get; set; }

    public decimal? BaseAtk { get; set; }

    public decimal? BaseDef { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public virtual StatType? AscensionStat { get; set; }

    public virtual ICollection<Build> Builds { get; set; } = new List<Build>();

    public virtual ICollection<CharacterConstellation> CharacterConstellations { get; set; } = new List<CharacterConstellation>();

    public virtual ICollection<CharacterTalent> CharacterTalents { get; set; } = new List<CharacterTalent>();

    public virtual ICollection<CharacterTranslation> CharacterTranslations { get; set; } = new List<CharacterTranslation>();

    public virtual Element Element { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<UserCharacter> UserCharacters { get; set; } = new List<UserCharacter>();

    public virtual WeaponType WeaponType { get; set; } = null!;
}
