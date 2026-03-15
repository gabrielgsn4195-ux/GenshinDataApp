using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class StatTypeHistory
{
    public int Id { get; set; }

    public int StatTypeId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
