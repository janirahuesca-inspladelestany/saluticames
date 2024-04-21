using Domain.CatalogueContext.Entities;
using Domain.CatalogueContext.Repositories;
using Domain.CatalogueContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence.CatalogueContext.Data;

namespace Persistence.CatalogueContext.Repositories;

public class CatalogueRepository : ICatalogueRepository
{
    private readonly CatalogueDbContext _catalogueDbContext;

    public CatalogueRepository(CatalogueDbContext catalogueDbContext)
    {
        _catalogueDbContext = catalogueDbContext;
    }

    public Catalogue? GetById(Guid id)
    {
        var catalogueEntity = _catalogueDbContext.Catalogue.Include(c => c.Summits).ThenInclude(s => s.Region).SingleOrDefault(c => c.Id == id);
        if (catalogueEntity is null) return null;

        var summitDetails = catalogueEntity.Summits.ToDictionary(summitEntity => summitEntity.Id, summitEntity =>
        {
            return new SummitDetails()
            {
                Altitude = summitEntity.Altitude,
                Location = summitEntity.Location,
                Name = summitEntity.Name,
                Region = summitEntity.Region.Name
            };
        });

        var catalogue = new Catalogue(catalogueEntity.Id) { Name = catalogueEntity.Name };
        var summits = catalogue.AddSummits(summitDetails);

        return catalogue;
    }

    public IEnumerable<Catalogue> GetAll()
    {
        var catalogueEntities = _catalogueDbContext.Catalogue.Include(c => c.Summits).ToList();
        if (!catalogueEntities.Any()) yield break;

        foreach (var catalogueEntity in catalogueEntities)
        {
            var catalogue = GetById(catalogueEntity.Id);
            if (catalogue is null) continue;
            yield return catalogue;
        }
    }

    public IEnumerable<string> GetAvailableRegions()
    {
        var availableRegions = _catalogueDbContext.Region.ToList();
        return availableRegions.Select(availableRegion => availableRegion.Name);
    }

    public void AddSummits(IEnumerable<Summit> summits)
    {
        var existingDifficulties = _catalogueDbContext.Difficulty.Select(d => d).ToList();
        var existingRegions = _catalogueDbContext.Region.Select(r => r).ToList();

        foreach (var summit in summits)
        {
            var summitEntity = new SummitEntity()
            {
                CatalogueId = summit.CatalogueId,
                Altitude = summit.SummitDetails.Altitude,
                DifficultyId = existingDifficulties.First(d => d.Id == (int)summit.SummitDetails.Difficulty).Id,
                Id = summit.Id,
                Location = summit.SummitDetails.Location,
                Name = summit.SummitDetails.Name,
                RegionId = existingRegions.First(r => r.Name.Equals(summit.SummitDetails.Region, StringComparison.OrdinalIgnoreCase)).Id
            };

            _catalogueDbContext.Summit.Add(summitEntity);
        }

        _catalogueDbContext.SaveChanges();
    }

    public void RemoveSummits(IEnumerable<Guid> summitIds)
    {
        var summitsToRemove = _catalogueDbContext.Summit.Where(s => summitIds.Contains(s.Id)).ToList();

        foreach (var summitToRemove in summitsToRemove)
        {
            _catalogueDbContext.Summit.Remove(summitToRemove);
        }

        _catalogueDbContext.SaveChanges();
    }

    public void EditSummits(IEnumerable<Summit> summits)
    {
        var existingDifficulties = _catalogueDbContext.Difficulty.Select(d => d).ToList();
        var existingRegions = _catalogueDbContext.Region.Select(r => r).ToList();

        foreach (var summit in summits)
        {
            var summitEntity = _catalogueDbContext.Summit.Find(summit.Id);
            if (summitEntity is null) continue;

            // TODO: ValueObjects are immutable objects!!

            summitEntity.CatalogueId = summit.CatalogueId;
            summitEntity.Altitude = summit.SummitDetails.Altitude;
            summitEntity.DifficultyId = existingDifficulties.First(d => d.Id == (int)summit.SummitDetails.Difficulty).Id;
            summitEntity.Id = summit.Id;
            summitEntity.Location = summit.SummitDetails.Location;
            summitEntity.Name = summit.SummitDetails.Name;
            summitEntity.RegionId = existingRegions.First(r => r.Name.Equals(summit.SummitDetails.Region, StringComparison.OrdinalIgnoreCase)).Id;

            //summitEntity = new SummitEntity
            //{
            //    CatalogueId = summit.CatalogueId,
            //    Altitude = summit.SummitDetails.Altitude,
            //    DifficultyId = existingDifficulties.First(d => d.Id == (int)summit.SummitDetails.Difficulty).Id,
            //    Id = summit.Id,
            //    Location = summit.SummitDetails.Location,
            //    Name = summit.SummitDetails.Name,
            //    RegionId = existingRegions.First(r => r.Name.Equals(summit.SummitDetails.Region, StringComparison.OrdinalIgnoreCase)).Id
            //};

            _catalogueDbContext.Summit.Update(summitEntity);
        }

        _catalogueDbContext.SaveChanges();
    }
}
