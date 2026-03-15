using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ArtifactPieceHistory
{
    public int Id { get; set; }

    public int PieceId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int ArtifactSetId { get; set; }

    public string SlotType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
