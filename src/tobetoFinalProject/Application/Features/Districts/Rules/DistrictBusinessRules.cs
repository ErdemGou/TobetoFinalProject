using Application.Features.Announcements.Constants;
using Application.Features.Districts.Constants;
using Application.Features.Districts.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Districts.Rules;

public class DistrictBusinessRules : BaseBusinessRules
{
    private readonly IDistrictRepository _districtRepository;

    public DistrictBusinessRules(IDistrictRepository districtRepository)
    {
        _districtRepository = districtRepository;
    }

    public Task DistrictShouldExistWhenSelected(District? district)
    {
        if (district == null)
            throw new BusinessException(DistrictsBusinessMessages.DistrictNotExists);
        return Task.CompletedTask;
    }

    public async Task DistrictIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        District? district = await _districtRepository.GetAsync(
            predicate: d => d.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await DistrictShouldExistWhenSelected(district);
    }

    public async Task DistrictShouldNotExistsWhenInsert(string name)
    {
        bool doesExists = await _districtRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(DistrictsBusinessMessages.DistrictNameExists);
    }
    public async Task DistrictShouldNotExistsWhenUpdate(string name)
    {
        bool doesExists = await _districtRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(DistrictsBusinessMessages.DistrictNameExists);
    }
}