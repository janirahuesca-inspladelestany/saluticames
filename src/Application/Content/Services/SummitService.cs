using Application.Abstractions;
using Contracts.DTO.Content;
using Domain.Content.Entities;
using Domain.Content.Enums;
using Domain.Content.Errors;
using SharedKernel.Common;
using SharedKernel.Helpers;

namespace Application.Content.Services;

public class SummitService(IUnitOfWork _unitOfWork) : ISummitService
{
    public async Task<Result<IEnumerable<Guid>, Error>> AddNewSummitsAsync(IEnumerable<AddNewSummitDto> summitDtos, CancellationToken cancellationToken = default)
    {
        // Recuperar els summits
        var existingSummits = await _unitOfWork.SummitRepository.ListAsync(
                filter: summit => summitDtos.Select(summitDto => summitDto.Name).Contains(summit.Name),
                cancellationToken: cancellationToken);

        if (existingSummits.Any()) return SummitErrors.SummitAlreadyExists;

        // Mapejar de DTO a BO
        var summitsToAdd = new List<SummitAggregate>();
        foreach (var summitDto in summitDtos)
        {
            if (!EnumHelper.TryGetEnumValueByDescription<Region>(summitDto.RegionName, out var region))
            {
                return SummitErrors.SummitRegionNotAvailable;
            }

            var summitCreateResult = SummitAggregate.Create(
                name: summitDto.Name,
                altitude: summitDto.Altitude,
                latitude: summitDto.Latitude,
                longitude: summitDto.Longitude,
                isEssential: summitDto.IsEssential,
                region: region);

            if (summitCreateResult.IsFailure()) return summitCreateResult.Error;
            var summitToAdd = summitCreateResult.Value!;

            // Afegir el summit
            await _unitOfWork.SummitRepository.AddAsync(summitToAdd);
            summitsToAdd.Add(summitToAdd);
        }

        if (summitsToAdd.Any())
        {
            // Persistir els summits
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return summitsToAdd.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IDictionary<Guid, ListSummitDetailDto>, Error>> ListSummitsAsync(ListSummitsFilterDto filterDto, CancellationToken cancellationToken = default)
    {
        // Recuperar els summits
        var summits = await _unitOfWork.SummitRepository.ListAsync(
            filter: summit =>
                (filterDto.Id != null ? summit.Id == filterDto.Id : true) &&
                (filterDto.Altitude.HasValue && (filterDto.Altitude.Value.Min != null || filterDto.Altitude.Value.Max != null)
                    ? (filterDto.Altitude.Value.Min ?? int.MinValue) <= summit.Altitude && summit.Altitude < (filterDto.Altitude.Value.Max ?? int.MaxValue) : true) &&
                (!string.IsNullOrEmpty(filterDto.Name) ? summit.Name.Contains(filterDto.Name) : true) &&
                (filterDto.IsEssential.HasValue ? summit.IsEssential == filterDto.IsEssential : true) &&
                (!string.IsNullOrEmpty(filterDto.RegionName) ? EnumHelper.GetDescription(summit.Region).Contains(filterDto.RegionName) : true) &&
                (!string.IsNullOrEmpty(filterDto.DifficultyLevel) ? EnumHelper.GetDescription(summit.DifficultyLevel).Contains(filterDto.DifficultyLevel) : true),
            cancellationToken: cancellationToken);

        // Mapejar de BO a DTO
        var result = summits.ToDictionary(summit => summit.Id, summit =>
            new ListSummitDetailDto(
                Name: summit.Name,
                Altitude: summit.Altitude,
                Latitude: summit.Latitude,
                Longitude: summit.Longitude,
                IsEssential: summit.IsEssential,
                RegionName: EnumHelper.GetDescription(summit.Region),
                DifficultyLevel: EnumHelper.GetDescription(summit.DifficultyLevel)));

        // Retornar el resultat
        return result;
    }

    public async Task<Result<IEnumerable<Guid>, Error>> ReplaceSummitsAsync(IDictionary<Guid, ReplaceSummitDetailDto> summitDtos, CancellationToken cancellationToken = default)
    {
        // Recuperar el summits
        var existingSummits = await _unitOfWork.SummitRepository.ListAsync(
                filter: summit => summitDtos.Keys.Select(key => key).Contains(summit.Id),
                cancellationToken: cancellationToken);

        // Actualizar summits
        var summitsToReplace = new List<SummitAggregate>();
        foreach (var summitDto in summitDtos)
        {
            var summitDetails = summitDto.Value;
            var summitToReplace = existingSummits.SingleOrDefault(existingSummit => existingSummit.Id == summitDto.Key);

            if (summitToReplace is null) return SummitErrors.SummitIdNotFound;

            if (!string.IsNullOrEmpty(summitDetails.Name))
            {
                var setNameResult = summitToReplace.SetName(summitDetails.Name);
                if (setNameResult.IsFailure()) return setNameResult.Error;
            }

            if (summitDetails.Altitude.HasValue)
            {
                var setAltitudeResult = summitToReplace.SetAltitude(summitDetails.Altitude.Value);
                if (setAltitudeResult.IsFailure()) return setAltitudeResult.Error;
            }

            if (summitDetails.Latitude.HasValue)
            {
                var setLatitudeResult = summitToReplace.SetLatitude(summitDetails.Latitude.Value);
                if (setLatitudeResult.IsFailure()) return setLatitudeResult.Error;
            }

            if (summitDetails.Longitude.HasValue)
            {
                var setLongitudeResult = summitToReplace.SetLongitude(summitDetails.Longitude.Value);
                if (setLongitudeResult.IsFailure()) return setLongitudeResult.Error;
            }

            if (summitDetails.IsEssential.HasValue)
            {
                var setIsEssentialResult = summitToReplace.SetIsEssential(summitDetails.IsEssential.Value);
                if (setIsEssentialResult.IsFailure()) return setIsEssentialResult.Error;
            }

            if (!string.IsNullOrEmpty(summitDetails.RegionName) &&
                EnumHelper.TryGetEnumValueByDescription<Region>(summitDetails.RegionName, out var region))
            {
                var setRegionResult = summitToReplace.SetRegion(region);
                if (setRegionResult.IsFailure()) return setRegionResult.Error;
            }

            summitsToReplace.Add(summitToReplace);
        }

        if (summitsToReplace.Any())
        {
            // Persistir els summits
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return existingSummits.Select(summit => summit.Id).ToList();
    }

    public async Task<Result<IEnumerable<Guid>, Error>> RemoveSummitsAsync(IEnumerable<Guid> summitIds,
        CancellationToken cancellationToken = default)
    {
        // Recuperar el summits
        var existingSummits = await _unitOfWork.SummitRepository.ListAsync(
                filter: summit => summitIds.Any(summitId => summitId == summit.Id),
                cancellationToken: cancellationToken);

        var summitsToRemove = new List<SummitAggregate>();
        foreach (var summitId in summitIds)
        {
            var summitToRemove = existingSummits.SingleOrDefault(existingSummit => existingSummit.Id == summitId);

            if (summitToRemove is null) return SummitErrors.SummitIdNotFound;

            // Eliminar els summits
            _unitOfWork.SummitRepository.Remove(summitToRemove);
            summitsToRemove.Add(summitToRemove);
        }

        if (summitsToRemove.Any())
        {
            // Persistir els summits
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Retornar el resultat
        return summitsToRemove.Select(summit => summit.Id).ToList();
    }
}
