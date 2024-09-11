using System.Collections.Generic;
using System.Threading.Tasks;
using RoboticsContainer.Core.Models;

namespace RoboticsContainer.Core.IRepositories
{
    public interface IContainerEntryRepository
    {
        Task<ContainerEntry> GetEntryByIdAsync(int id);
        Task<IEnumerable<ContainerEntry>> GetAllEntriesAsync();
        Task AddEntryAsync(ContainerEntry entry);
        Task UpdateEntryAsync(ContainerEntry entry);
        Task DeleteEntryAsync(int id);
    }
}
