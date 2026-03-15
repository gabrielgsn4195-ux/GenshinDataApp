using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserCharacter
{
    public int Id { get; set; }

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

    public virtual Character Character { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
