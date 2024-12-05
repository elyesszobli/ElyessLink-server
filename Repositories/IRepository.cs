namespace ElyessLink_API.Repositories
{
    public interface IRepository<TEntity>
    {
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        TEntity Create(TEntity entity);
        void Delete (TEntity entity);


    }
}
