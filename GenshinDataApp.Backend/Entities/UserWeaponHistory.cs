using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserWeaponHistory
{
    public int Id { get; set; }

    public int UserWeaponId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int WeaponId { get; set; }

    public int Level { get; set; }

    public int AscensionPhase { get; set; }

    public int Refinement { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
