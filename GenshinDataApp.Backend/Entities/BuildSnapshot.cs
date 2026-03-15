using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BuildSnapshot
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int BuildId { get; set; }

    public int Version { get; set; }

    public string SnapshotJson { get; set; } = null!;

    public DateTime ArchivedAt { get; set; }

    public virtual Build Build { get; set; } = null!;
}
