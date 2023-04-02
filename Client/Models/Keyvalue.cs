using System;
using System.Collections.Generic;

namespace Client.Models;

public partial class KeyValue
{
    public int Id { get; set; }

    public int Key { get; set; }

    public string Value { get; set; } = null!;
}
