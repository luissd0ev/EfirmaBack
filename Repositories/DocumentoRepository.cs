using APIEfirma.Models;

namespace APIEfirma.Repositories
{
    public class DocumentoRepository<T> : Repository<T>, IDocumento<T> where T : class
    {
        public EfirmaDbContext context;

        public DocumentoRepository(EfirmaDbContext context) : base(context)
        {
            this.context = context;
        }

    }
}
