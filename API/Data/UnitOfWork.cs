using API.Data.Repository;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UnitOfWork(DataContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            _context = context;
            _hostEnvironment = hostEnvironment;

        }
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IPermissionRepository PermissionRepository => new PermissionRepository(_context, _mapper);
        public IConfigurationRepository ConfigurationRepository => new ConfigurationRepository(_context, _mapper);
        public IFlightRepository FlightRepository => new FlightRepository(_context, _mapper);
        public IDocumentRepository DocumentRepository => new DocumentRepository(_context, _mapper, _hostEnvironment);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
