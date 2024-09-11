using RoboticsContainer.Core.Models;
using RoboticsContainer.Application.DTOs;  

namespace RoboticsContainer.Application.Interfaces
{
    public interface IContainerEntryService
    {
        Task<ContainerEntry> CreateEntryAsync(ContainerEntryRequestDTO request);
        Task<ContainerEntry> GetEntryByIdAsync(int id);
        Task<IEnumerable<ContainerEntry>> GetAllEntriesAsync();
        Task UpdateEntryAsync(int id, ContainerEntryRequestDTO request);
        Task DeleteEntryAsync(int id);
        Task<Stream> ExportEntriesToCsvAsync();
    }
}
