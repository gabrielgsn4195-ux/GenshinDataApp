using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class RegionTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int RegionId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual Region Region { get; set; } = null!;
}
