using APIEfirma.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace APIEfirma.Repositories
{
    public class DocumentoRepository<T> : Repository<T>, IDocumento<T> where T : class
    {
        public EfirmaDbContext context;

        public DocumentoRepository(EfirmaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> FindDocumentAsync(string hashCode)
        {
            return await context.Documentos.AnyAsync(documento => documento.DocHashcode == hashCode);
        }

    }
}
