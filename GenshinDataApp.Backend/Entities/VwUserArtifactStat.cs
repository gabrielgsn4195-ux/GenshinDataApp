using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class VwUserArtifactStat
{
    public int UserId { get; set; }

    public int UserArtifactId { get; set; }

    public int ArtifactPieceId { get; set; }

    public int MainStatId { get; set; }

    public decimal MainStatValue { get; set; }

    public int? StatId { get; set; }

    public decimal? StatValue { get; set; }
}
