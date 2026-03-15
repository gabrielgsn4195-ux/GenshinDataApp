using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BugReportImageHistory
{
    public int Id { get; set; }

    public int ImageId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int BugReportId { get; set; }

    public string ImagePath { get; set; } = null!;

    public string? FileName { get; set; }

    public int? FileSizeBytes { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
