using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class ElementHistory
{
    public int Id { get; set; }

    public int ElementId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Name { get; set; } = null!;

    public string? IconPath { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
