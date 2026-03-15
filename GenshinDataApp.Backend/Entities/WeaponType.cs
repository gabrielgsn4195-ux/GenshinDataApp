using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponType
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? IconPath { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<WeaponTypeTranslation> WeaponTypeTranslations { get; set; } = new List<WeaponTypeTranslation>();

    public virtual ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
}
