using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponRefinementHistory
{
    public int Id { get; set; }

    public int RefinementId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int WeaponId { get; set; }

    public int RefinementLevel { get; set; }

    public string Description { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
