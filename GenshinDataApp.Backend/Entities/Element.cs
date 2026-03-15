using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class Element
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? IconPath { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<ElementTranslation> ElementTranslations { get; set; } = new List<ElementTranslation>();
}
