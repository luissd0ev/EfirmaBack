using APIEfirma.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace APIEfirma.Repositories
{
    public class EfirmaRepository<T> : Repository<T>, IEfirma<T> where T : class
    {
        public EfirmaDbContext context;

        public EfirmaRepository(EfirmaDbContext context): base(context) 
        { 
            this.context = context;
        }

       
    }
}
