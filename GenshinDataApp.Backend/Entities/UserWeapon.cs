using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserWeapon
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int WeaponId { get; set; }

    public int Level { get; set; }

    public int AscensionPhase { get; set; }

    public int Refinement { get; set; }

    public virtual ICollection<Build> Builds { get; set; } = new List<Build>();

    public virtual User User { get; set; } = null!;

    public virtual Weapon Weapon { get; set; } = null!;
}
