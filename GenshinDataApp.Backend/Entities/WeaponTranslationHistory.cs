using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponTranslationHistory
{
    public int Id { get; set; }

    public int TranslationId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int WeaponId { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PassiveName { get; set; }

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
