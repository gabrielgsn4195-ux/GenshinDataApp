using System;
using System.Collections.Generic;

namespace GenshinDataApp.Backend.Entities;

public partial class EmailRateLimit
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string ActionType { get; set; } = null!;

    public int RequestCount { get; set; }

    public DateTime WindowStart { get; set; }

    public DateTime LastRequest { get; set; }
}
