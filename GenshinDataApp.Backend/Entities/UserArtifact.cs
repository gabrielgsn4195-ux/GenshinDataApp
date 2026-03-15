using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class UserArtifact
{
    public int Id { get; set; }

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

    public virtual ArtifactPiece ArtifactPiece { get; set; } = null!;

    public virtual ICollection<Build> BuildArtifactCirclets { get; set; } = new List<Build>();

    public virtual ICollection<Build> BuildArtifactFlowers { get; set; } = new List<Build>();

    public virtual ICollection<Build> BuildArtifactGoblets { get; set; } = new List<Build>();

    public virtual ICollection<Build> BuildArtifactPlumes { get; set; } = new List<Build>();

    public virtual ICollection<Build> BuildArtifactSands { get; set; } = new List<Build>();

    public virtual StatType MainStat { get; set; } = null!;

    public virtual StatType? SubStat1 { get; set; }

    public virtual StatType? SubStat2 { get; set; }

    public virtual StatType? SubStat3 { get; set; }

    public virtual StatType? SubStat4 { get; set; }

    public virtual User User { get; set; } = null!;
}
