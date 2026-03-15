using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterTalent
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int CharacterId { get; set; }

    public string Name { get; set; } = null!;

    public string TalentType { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconPath { get; set; }

    public int SortOrder { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual ICollection<CharacterTalentTranslation> CharacterTalentTranslations { get; set; } = new List<CharacterTalentTranslation>();
}
