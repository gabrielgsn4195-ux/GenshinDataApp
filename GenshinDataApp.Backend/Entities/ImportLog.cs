using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ImportLog
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string EntityType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? TotalRecords { get; set; }

    public int? InsertedCount { get; set; }

    public int? UpdatedCount { get; set; }

    public int? ErrorCount { get; set; }

    public string? ErrorDetails { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int ExecutedByUserId { get; set; }

    public virtual User ExecutedByUser { get; set; } = null!;
}
