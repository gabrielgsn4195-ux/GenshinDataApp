using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class CharacterHistory
{
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? Title { get; set; }

    public int ElementId { get; set; }

    public int Rarity { get; set; }

    public int RegionId { get; set; }

    public int WeaponTypeId { get; set; }

    public string? Description { get; set; }

    public int? AscensionStatId { get; set; }

    public decimal? BaseHp { get; set; }

    public decimal? BaseAtk { get; set; }

    public decimal? BaseDef { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
