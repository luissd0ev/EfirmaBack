namespace APIEfirma.Repositories
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T>? GetAll();

        public int Insert(T entity);

        public T? Get(params object[] keyValues);
    }
}
