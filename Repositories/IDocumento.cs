namespace APIEfirma.Repositories
{
    public interface IDocumento <T> : IRepository<T> where T : class
    {
        public Task<bool> FindDocumentAsync(string hashCode); 
    }
}
