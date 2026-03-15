using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ArtifactSet
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public int MaxRarity { get; set; }

    public string? TwoPieceBonus { get; set; }

    public string? FourPieceBonus { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<ArtifactPiece> ArtifactPieces { get; set; } = new List<ArtifactPiece>();

    public virtual ICollection<ArtifactSetTranslation> ArtifactSetTranslations { get; set; } = new List<ArtifactSetTranslation>();
}
