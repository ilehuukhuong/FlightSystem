namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IConfigurationRepository ConfigurationRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
