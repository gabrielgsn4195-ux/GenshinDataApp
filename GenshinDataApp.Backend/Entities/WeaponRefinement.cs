using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponRefinement
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int WeaponId { get; set; }

    public int RefinementLevel { get; set; }

    public string Description { get; set; } = null!;

    public virtual Weapon Weapon { get; set; } = null!;

    public virtual ICollection<WeaponRefinementTranslation> WeaponRefinementTranslations { get; set; } = new List<WeaponRefinementTranslation>();
}
