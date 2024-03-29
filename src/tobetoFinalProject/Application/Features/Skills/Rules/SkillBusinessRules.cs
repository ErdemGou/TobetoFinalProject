using Application.Features.Languages.Constants;
using Application.Features.Skills.Constants;
using Application.Features.Skills.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Skills.Rules;

public class SkillBusinessRules : BaseBusinessRules
{
    private readonly ISkillRepository _skillRepository;

    public SkillBusinessRules(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public Task SkillShouldExistWhenSelected(Skill? skill)
    {
        if (skill == null)
            throw new BusinessException(SkillsBusinessMessages.SkillNotExists);
        return Task.CompletedTask;
    }

    public async Task SkillIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Skill? skill = await _skillRepository.GetAsync(
            predicate: s => s.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await SkillShouldExistWhenSelected(skill);
    }

    public async Task SkillShouldNotExistsWhenInsert(string name)
    {
        bool doesExists = await _skillRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(SkillsBusinessMessages.SkillNameExists);
    }
    public async Task SkillShouldNotExistsWhenUpdate(string name)
    {
        bool doesExists = await _skillRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(SkillsBusinessMessages.SkillNameExists);
    }

}