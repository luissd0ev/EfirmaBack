using System;
using System.Collections.Generic;

namespace APIEfirma.Models;

public partial class Documento
{
    public int DocId { get; set; }

    public string DocRfc { get; set; } = null!;

    public string DocNombredocumento { get; set; } = null!;

    public string? DocFirma { get; set; }

    public string? DocHashcode { get; set; }
}
