using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Weapon
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public int WeaponTypeId { get; set; }

    public int Rarity { get; set; }

    public string? Description { get; set; }

    public decimal? BaseAtk { get; set; }

    public int? SecondaryStatId { get; set; }

    public decimal? SecondaryStatValue { get; set; }

    public string? PassiveName { get; set; }

    public string? ImagePath { get; set; }

    public string? ThumbnailPath { get; set; }

    public virtual StatType? SecondaryStat { get; set; }

    public virtual ICollection<UserWeapon> UserWeapons { get; set; } = new List<UserWeapon>();

    public virtual ICollection<WeaponRefinement> WeaponRefinements { get; set; } = new List<WeaponRefinement>();

    public virtual ICollection<WeaponTranslation> WeaponTranslations { get; set; } = new List<WeaponTranslation>();

    public virtual WeaponType WeaponType { get; set; } = null!;
}
