using API.DTOs;
using API.Entities;
using API.Helpers;


namespace API.Interfaces
{
    public interface IDocumentRepository
    {
        Task CreateDocument(DocumentDto documentDto);
        bool DeleteDocument(int id);
        Task UpdateDocument(DocumentDto updateDocumentDto);
        Task<DocumentDto> GetDocumentAsync(int id);
        Task<PagedList<DocumentDto>> GetDocumentsAsync(PaginationParams paginationParams);
        Task<Document> GetDocumentFileAsync(int id);
    }
}
