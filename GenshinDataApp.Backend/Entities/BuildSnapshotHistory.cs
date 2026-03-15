using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BuildSnapshotHistory
{
    public int Id { get; set; }

    public int SnapshotId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int BuildId { get; set; }

    public int Version { get; set; }

    public DateTime ArchivedAt { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
