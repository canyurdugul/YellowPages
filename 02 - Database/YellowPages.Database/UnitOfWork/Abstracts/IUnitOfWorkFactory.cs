namespace YellowPages.Database.UnitOfWork.Abstracts
{
    public interface IUnitOfWorkFactory
    {
        TDbContext GetDbContextFromStack<TDbContext>();
        IUnitOfWork Create();
        IUnitOfWork CreateWithStack();
    }
}
