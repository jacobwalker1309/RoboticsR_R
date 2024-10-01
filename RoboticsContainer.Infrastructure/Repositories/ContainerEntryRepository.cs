using Microsoft.EntityFrameworkCore;
using RoboticsContainer.Infrastructure.Data;
using RoboticsContainer.Core.Models;
using RoboticsContainer.Core.IRepositories;
using RoboticsContainer.Application.Interfaces;

namespace RoboticsContainer.Infrastructure.Repositories
{
    public class ContainerEntryRepository : IContainerEntryRepository
    {
        // Next work is to check for null within the service for all of these and then do another check for null here too
        // More error handling
        private readonly AppDbContext _context;

        public ContainerEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContainerEntry> GetEntryByIdAsync(int id)
        {
            return await _context.ContainerEntries.FindAsync(id);
        }

        public async Task<IEnumerable<ContainerEntry>> GetAllEntriesAsync()
        {
            return await _context.ContainerEntries.ToListAsync();
        }

        public async Task AddEntryAsync(ContainerEntry entry)
        {
            await _context.ContainerEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntryAsync(ContainerEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntryAsync(int id)
        {
            var entry = await _context.ContainerEntries.FindAsync(id);
            if (entry != null)
            {
                _context.ContainerEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ContainerEntry>> FindAsync(ISpecification<ContainerEntry> specification)
        {
            return await _context.ContainerEntries
                .Where(specification.ToExpression())
                .ToListAsync();
        }

    }
}
