using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BugReportHistory
{
    public int Id { get; set; }

    public int BugReportId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Priority { get; set; }

    public string? PublicTitle { get; set; }

    public string? PublicDescription { get; set; }

    public int? AssignedToUserId { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
