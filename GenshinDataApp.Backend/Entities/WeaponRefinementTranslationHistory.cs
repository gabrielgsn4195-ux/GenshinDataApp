using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class WeaponRefinementTranslationHistory
{
    public int Id { get; set; }

    public int TranslationId { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int WeaponRefinementId { get; set; }

    public int LanguageId { get; set; }

    public string Description { get; set; } = null!;

    public string OperationType { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public string OperationUser { get; set; } = null!;
}
