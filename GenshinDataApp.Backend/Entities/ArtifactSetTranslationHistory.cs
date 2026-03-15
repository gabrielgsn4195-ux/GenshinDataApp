using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ArtifactSetTranslationHistory
{
    public int Id { get; set; }

    public int TranslationId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int ArtifactSetId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? TwoPieceBonus { get; set; }

    public string? FourPieceBonus { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
