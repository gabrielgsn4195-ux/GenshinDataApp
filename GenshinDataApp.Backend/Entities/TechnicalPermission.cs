using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class TechnicalPermission
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public string Permission { get; set; } = null!;

    public int GrantedByUserId { get; set; }

    public DateTime GrantedAt { get; set; }

    public virtual User GrantedByUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
