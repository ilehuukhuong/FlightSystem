using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public DocumentRepository(DataContext context, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mapper = mapper;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task CreateDocument(DocumentDto documentDto)
        {
            var file = documentDto.File;
            if (file != null && file.Length > 0)
            {
                // Tạo đường dẫn lưu tệp dựa vào thư mục wwwroot
                var uploadsFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                    Directory.CreateDirectory(uploadsFolderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Tạo và cập nhật thông tin document
                var document = _mapper.Map<Document>(documentDto);
                document.PathFile = filePath;
                document.Version = 1.0;

                _context.Documents.Add(document);
            }
        }

        public bool DeleteDocument(int id)
        {
            var document = _context.Documents.Where(x => x.Id == id).SingleOrDefault();

            if (document == null)
            {
                return false;
            }

            _context.Documents.Remove(document);

            return true;
        }

        public async Task<DocumentDto> GetDocumentAsync(int id)
        {
            return _mapper.Map<DocumentDto>(await _context.Documents.FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task<Document> GetDocumentFileAsync(int id)
        {
            return await _context.Documents.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PagedList<DocumentDto>> GetDocumentsAsync(PaginationParams paginationParams)
        {
            var query = _context.Documents.AsQueryable();

            return await PagedList<DocumentDto>.CreateAsync(query.AsNoTracking().ProjectTo<DocumentDto>(_mapper.ConfigurationProvider), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task UpdateDocument(DocumentDto documentDto)
        {
            var file = documentDto.File;
            if (file != null && file.Length > 0)
            {
                // Tạo đường dẫn lưu tệp dựa vào thư mục wwwroot
                var uploadsFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                    Directory.CreateDirectory(uploadsFolderPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Tạo và cập nhật thông tin document
                var document = _context.Documents.Where(x => x.Id == documentDto.Id).FirstOrDefault();
                document.PathFile = filePath;
                documentDto.Version = document.Version + 0.1;
                _mapper.Map(documentDto, document);
            }         
        }
    }
}
