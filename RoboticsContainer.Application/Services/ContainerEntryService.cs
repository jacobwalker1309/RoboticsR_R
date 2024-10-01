using System.Globalization;
using AutoMapper;
using CsvHelper;
using RoboticsContainer.Application.Interfaces;
using RoboticsContainer.Application.DTOs;
using RoboticsContainer.Core.IRepositories;
using RoboticsContainer.Core.Models;
using RoboticsContainer.Application.Specification.ContainerEntrySpecification;
using RoboticsContainer.Application.Extensions;
using static RoboticsContainer.Application.Specification.SpecificationLayoutComponents.SpecificationFilteringOperators;

namespace RoboticsContainer.Services
{
    public class ContainerEntryService : IContainerEntryService
    {
        private readonly IContainerEntryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ContainerEntryService(IContainerEntryRepository repository, IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ContainerEntry> CreateEntryAsync(ContainerEntryRequestDTO request)
        {
            var entry = _mapper.Map<ContainerEntry>(request);
            entry.DateInserted = DateTime.UtcNow;

            await _repository.AddEntryAsync(entry);

            // Cache the new entry
            await _cacheService.SetAsync($"ContainerEntry_{entry.ID}", entry, TimeSpan.FromHours(1));

            // Update the cached list of IDs
            var cachedEntryIds = await _cacheService.GetAsync<List<int>>("AllContainerEntryIds");
            if (cachedEntryIds != null)
            {
                cachedEntryIds.Add(entry.ID);
                await _cacheService.SetAsync("AllContainerEntryIds", cachedEntryIds, TimeSpan.FromHours(1));
            }

            return entry;
        }

        public async Task<ContainerEntry> GetEntryByIdAsync(int id)
        {
            var cacheKey = $"ContainerEntry_{id}";
            var cachedEntry = await _cacheService.GetAsync<ContainerEntry>(cacheKey);

            if (cachedEntry != null)
            {
                return cachedEntry;
            }

            var entry = await _repository.GetEntryByIdAsync(id);

            if (entry != null)
            {
                await _cacheService.SetAsync(cacheKey, entry, TimeSpan.FromHours(1));
            }

            return entry;
        }

        public async Task<IEnumerable<ContainerEntry>> GetAllEntriesAsync()
        {
            var cacheKey = "AllContainerEntryIds";
            var cachedEntryIds = await _cacheService.GetAsync<List<int>>(cacheKey);

            List<ContainerEntry> entries;

            if (cachedEntryIds != null)
            {
                entries = new List<ContainerEntry>();
                foreach (var id in cachedEntryIds)
                {
                    var entryCacheKey = $"ContainerEntry_{id}";
                    var cachedEntry = await _cacheService.GetAsync<ContainerEntry>(entryCacheKey);
                    if (cachedEntry != null)
                    {
                        entries.Add(cachedEntry);
                    }
                    else
                    {
                        // If any entry is not in cache, fall back to repository
                        var entryFromDb = await _repository.GetEntryByIdAsync(id);
                        if (entryFromDb != null)
                        {
                            entries.Add(entryFromDb);
                            await _cacheService.SetAsync(entryCacheKey, entryFromDb, TimeSpan.FromHours(1));
                        }
                    }
                }
            }
            else
            {
                // If the list of IDs is not cached, get all entries from the database
                entries = (await _repository.GetAllEntriesAsync()).ToList();

                if (entries.Any())
                {
                    // Cache the list of IDs
                    var entryIds = entries.Select(e => e.ID).ToList();
                    await _cacheService.SetAsync(cacheKey, entryIds, TimeSpan.FromHours(1));

                    // Cache individual entries
                    foreach (var entry in entries)
                    {
                        var entryCacheKey = $"ContainerEntry_{entry.ID}";
                        await _cacheService.SetAsync(entryCacheKey, entry, TimeSpan.FromHours(1));
                    }
                }
            }

            return entries;
        }

        public async Task UpdateEntryAsync(int id, ContainerEntryRequestDTO request)
        {
            var entry = await _repository.GetEntryByIdAsync(id);
            if (entry != null)
            {
                _mapper.Map(request, entry);  // Map the changes from the request to the existing entry
                await _repository.UpdateEntryAsync(entry);

                // Update the cache for the entry
                await _cacheService.SetAsync($"ContainerEntry_{id}", entry, TimeSpan.FromHours(1));
            }
        }

        public async Task DeleteEntryAsync(int id)
        {
            await _repository.DeleteEntryAsync(id);

            // Remove the entry from cache
            var cacheKey = $"ContainerEntry_{id}";
            await _cacheService.RemoveAsync(cacheKey);

            // Update the cached list of IDs
            var cachedEntryIds = await _cacheService.GetAsync<List<int>>("AllContainerEntryIds");
            if (cachedEntryIds != null)
            {
                cachedEntryIds.Remove(id);
                await _cacheService.SetAsync("AllContainerEntryIds", cachedEntryIds, TimeSpan.FromHours(1));
            }
        }

        public async Task<Stream> ExportEntriesToCsvAsync()
        {
            var entries = await _repository.GetAllEntriesAsync();

            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Write the column headers
                csv.WriteHeader<ContainerEntry>();
                await csv.NextRecordAsync();

                // Write the records
                await csv.WriteRecordsAsync(entries);
                await writer.FlushAsync();
            }

            memoryStream.Position = 0; // Reset the stream position to the beginning
            return memoryStream;
        }

        // note to self, this method can be imporved upon via importing a dictionary as a parameter and using reflection 
        // to grab the variables and then creating specification based on the reflections variable choice
        // will prevent the need for adding to this method every time container entry is extended
        // cons - very complex and will take about 2 weeks - a month to get right. 

        public async Task<IEnumerable<ContainerEntry>> GetFilteredEntriesAsync(
        float? minTemperature, float? maxTemperature,
        float? minVoltage, float? maxVoltage,
        float? minCurrent, float? maxCurrent,
        float? minStateOfCharge, float? maxStateOfCharge,
        DateTime? minDateInserted, DateTime? maxDateInserted,
        int? containerId)
        {
            ISpecification<ContainerEntry>? spec = new TrueSpecification<ContainerEntry>(); // Base specification

            if (minTemperature.HasValue || maxTemperature.HasValue)
            {
                spec = spec.And(new TemperatureSpecification(minTemperature, maxTemperature));
            }

            if (minVoltage.HasValue || maxVoltage.HasValue)
            {
                spec = spec.And(new VoltageSpecification(minVoltage, maxVoltage));
            }

            if (minCurrent.HasValue || maxCurrent.HasValue)
            {
                spec = spec.And(new CurrentSpecification(minCurrent, maxCurrent));
            }

            if (minStateOfCharge.HasValue || maxStateOfCharge.HasValue)
            {
                spec = spec.And(new StateOfChargeSpecification(minStateOfCharge, maxStateOfCharge));
            }

            if (minDateInserted.HasValue || maxDateInserted.HasValue)
            {
                spec = spec.And(new DateInsertedSpecification(minDateInserted, maxDateInserted));
            }

            if (containerId.HasValue)
            {
                spec = spec.And(new ContainerIDSpecification(containerId.Value));
            }

            return await _repository.FindAsync(spec);
        }
    }
}
