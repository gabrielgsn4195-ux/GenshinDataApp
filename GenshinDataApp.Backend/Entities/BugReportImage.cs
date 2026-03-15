using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BugReportImage
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int BugReportId { get; set; }

    public string ImagePath { get; set; } = null!;

    public string? FileName { get; set; }

    public int? FileSizeBytes { get; set; }

    public virtual BugReport BugReport { get; set; } = null!;
}
