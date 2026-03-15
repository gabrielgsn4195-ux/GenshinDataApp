using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int CharacterId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
