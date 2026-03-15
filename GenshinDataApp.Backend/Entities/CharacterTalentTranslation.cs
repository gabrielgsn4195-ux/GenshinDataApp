using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterTalentTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int CharacterTalentId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual CharacterTalent CharacterTalent { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
