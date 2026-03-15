using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ArtifactPiece
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int ArtifactSetId { get; set; }

    public string SlotType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<ArtifactPieceTranslation> ArtifactPieceTranslations { get; set; } = new List<ArtifactPieceTranslation>();

    public virtual ArtifactSet ArtifactSet { get; set; } = null!;

    public virtual ICollection<UserArtifact> UserArtifacts { get; set; } = new List<UserArtifact>();
}
