using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Keyvalue
{
    public int Id { get; set; }

    public int Key { get; set; }

    public string Value { get; set; } = null!;
}
