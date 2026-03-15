using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponRefinementTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int WeaponRefinementId { get; set; }

    public int LanguageId { get; set; }

    public string Description { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual WeaponRefinement WeaponRefinement { get; set; } = null!;
}
