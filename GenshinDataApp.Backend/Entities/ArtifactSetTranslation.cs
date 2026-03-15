using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ArtifactSetTranslation
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int ArtifactSetId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? TwoPieceBonus { get; set; }

    public string? FourPieceBonus { get; set; }

    public virtual ArtifactSet ArtifactSet { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
