using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterTalentHistory
{
    public int Id { get; set; }

    public int TalentId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int CharacterId { get; set; }

    public string Name { get; set; } = null!;

    public string TalentType { get; set; } = null!;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
