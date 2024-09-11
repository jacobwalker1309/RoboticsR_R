using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoboticsContainer.Infrastructure.Data;
using RoboticsContainer.Core.Models;
using RoboticsContainer.Services;
using RoboticsContainer.Core.IRepositories;

namespace RoboticsContainer.Infrastructure.Repositories
{
    public class ContainerEntryRepository : IContainerEntryRepository
    {
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
    }
}
