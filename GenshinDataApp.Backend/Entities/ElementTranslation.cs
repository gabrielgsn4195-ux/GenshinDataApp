using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ElementTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int ElementId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Element Element { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
