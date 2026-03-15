using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class BugReport
{
    public int Id { get; set; }

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

    public DateTime CreatedAt { get; set; }

    public virtual User? AssignedToUser { get; set; }

    public virtual ICollection<BugReportImage> BugReportImages { get; set; } = new List<BugReportImage>();

    public virtual User User { get; set; } = null!;
}
