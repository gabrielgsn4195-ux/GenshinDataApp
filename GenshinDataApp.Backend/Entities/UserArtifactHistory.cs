using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserArtifactHistory
{
    public int Id { get; set; }

    public int UserArtifactId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public int ArtifactPieceId { get; set; }

    public int Level { get; set; }

    public int Rarity { get; set; }

    public int MainStatId { get; set; }

    public decimal MainStatValue { get; set; }

    public int? SubStat1Id { get; set; }

    public decimal? SubStat1Value { get; set; }

    public int? SubStat2Id { get; set; }

    public decimal? SubStat2Value { get; set; }

    public int? SubStat3Id { get; set; }

    public decimal? SubStat3Value { get; set; }

    public int? SubStat4Id { get; set; }

    public decimal? SubStat4Value { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
