using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using Dapper;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Collections;
using QRCoder;
using System.Drawing;
using FirmaDocumento.Areas.FEA.Models;
using System.Net.Http;
using Abp.Threading;


namespace FirmaDocumento.Areas.FEA.Utility
{
    public class Constantes
    {
        public static readonly string AnioActiveDirectoryActual = "2021";
        public static readonly int SizeList = 10;
    }
}