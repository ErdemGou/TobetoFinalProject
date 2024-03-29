using Application.Features.Languages.Constants;
using Application.Features.Manufacturers.Constants;
using Application.Features.Manufacturers.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Manufacturers.Rules;

public class ManufacturerBusinessRules : BaseBusinessRules
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerBusinessRules(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public Task ManufacturerShouldExistWhenSelected(Manufacturer? manufacturer)
    {
        if (manufacturer == null)
            throw new BusinessException(ManufacturersBusinessMessages.ManufacturerNotExists);
        return Task.CompletedTask;
    }

    public async Task ManufacturerIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Manufacturer? manufacturer = await _manufacturerRepository.GetAsync(
            predicate: m => m.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await ManufacturerShouldExistWhenSelected(manufacturer);
    }

    public async Task ManufacturerShouldNotExistsWhenInsert(string name)
    {
        bool doesExists = await _manufacturerRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(ManufacturersBusinessMessages.ManufacturerNameExists);
    }
    public async Task ManufacturerShouldNotExistsWhenUpdate(string name)
    {
        bool doesExists = await _manufacturerRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(ManufacturersBusinessMessages.ManufacturerNameExists);
    }
}