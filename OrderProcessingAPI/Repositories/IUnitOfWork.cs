namespace OrderProcessingAPI.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
