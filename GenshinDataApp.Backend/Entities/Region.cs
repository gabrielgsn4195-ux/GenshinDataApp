using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Region
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? IconPath { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<RegionTranslation> RegionTranslations { get; set; } = new List<RegionTranslation>();
}
