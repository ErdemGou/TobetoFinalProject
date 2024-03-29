using Application.Features.Announcements.Constants;
using Application.Features.Announcements.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.Announcements.Rules;

public class AnnouncementBusinessRules : BaseBusinessRules
{
    private readonly IAnnouncementRepository _announcementRepository;

    public AnnouncementBusinessRules(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }

    public Task AnnouncementShouldExistWhenSelected(Announcement? announcement)
    {
        if (announcement == null)
            throw new BusinessException(AnnouncementsBusinessMessages.AnnouncementNotExists);
        return Task.CompletedTask;
    }

    public async Task AnnouncementIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Announcement? announcement = await _announcementRepository.GetAsync(
            predicate: a => a.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await AnnouncementShouldExistWhenSelected(announcement);
    }

    public async Task AnnouncementShouldNotExistsWhenInsert(string name)
    {
        bool doesExists = await _announcementRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(AnnouncementsBusinessMessages.AnnouncementNameExists);
    }
    public async Task AnnouncementShouldNotExistsWhenUpdate(string name)
    {
        bool doesExists = await _announcementRepository
            .AnyAsync(predicate: ca => ca.Name == name, enableTracking: false);
        if (doesExists)
            throw new BusinessException(AnnouncementsBusinessMessages.AnnouncementNameExists);
    }
}