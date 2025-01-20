using APIEfirma.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace APIEfirma.Repositories
{
    public class EfirmaRepository:IEfirma
    {
        public EfirmaDbContext context;

        public EfirmaRepository(EfirmaDbContext context) { 
            this.context = context;
        }

        public async Task<dynamic> getDocuments()
        {
            var consulta = await (from doc in context.Documentos
                                  select new
                                  {
                                      RFC = doc.DocRfc,
                                      Hash = doc.DocHashcode
                                  }
                                  ).ToListAsync(); 

            return consulta;
        }
    }
}
