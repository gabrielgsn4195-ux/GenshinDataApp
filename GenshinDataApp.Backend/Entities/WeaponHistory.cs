using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponHistory
{
    public int Id { get; set; }

    public int WeaponId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public int WeaponTypeId { get; set; }

    public int Rarity { get; set; }

    public string? Description { get; set; }

    public decimal? BaseAtk { get; set; }

    public int? SecondaryStatId { get; set; }

    public decimal? SecondaryStatValue { get; set; }

    public string? PassiveName { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
