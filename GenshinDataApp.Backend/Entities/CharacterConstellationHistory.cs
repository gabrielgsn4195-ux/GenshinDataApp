using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterConstellationHistory
{
    public int Id { get; set; }

    public int ConstellationId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int CharacterId { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
