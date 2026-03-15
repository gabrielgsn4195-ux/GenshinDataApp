using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class LanguageHistory
{
    public int Id { get; set; }

    public int LanguageId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NativeName { get; set; } = null!;

    public bool IsDefault { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
