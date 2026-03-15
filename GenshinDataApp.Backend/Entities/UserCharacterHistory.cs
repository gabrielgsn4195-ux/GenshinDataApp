using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserCharacterHistory
{
    public int Id { get; set; }

    public int UserCharacterId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int CharacterId { get; set; }

    public int Level { get; set; }

    public int AscensionPhase { get; set; }

    public int Constellation { get; set; }

    public int TalentNormalAtk { get; set; }

    public int TalentSkill { get; set; }

    public int TalentBurst { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
