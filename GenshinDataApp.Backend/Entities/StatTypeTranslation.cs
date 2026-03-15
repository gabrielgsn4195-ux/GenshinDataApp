using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class StatTypeTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int StatTypeId { get; set; }

    public int LanguageId { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;

    public virtual StatType StatType { get; set; } = null!;
}
