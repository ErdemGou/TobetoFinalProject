using Application.Features.Announcements.Constants;
using Application.Features.Announcements.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Announcements.Constants.AnnouncementsOperationClaims;

namespace Application.Features.Announcements.Commands.Create;

public class CreateAnnouncementCommand : IRequest<CreatedAnnouncementResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<ClassAnnouncement>? ClassAnnouncements { get; set; }

    public string[] Roles => new[] { Admin, Write, AnnouncementsOperationClaims.Create };

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string CacheGroupKey => "GetAnnouncements";

    public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, CreatedAnnouncementResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly AnnouncementBusinessRules _announcementBusinessRules;

        public CreateAnnouncementCommandHandler(IMapper mapper, IAnnouncementRepository announcementRepository,
                                         AnnouncementBusinessRules announcementBusinessRules)
        {
            _mapper = mapper;
            _announcementRepository = announcementRepository;
            _announcementBusinessRules = announcementBusinessRules;
        }

        public async Task<CreatedAnnouncementResponse> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            Announcement announcement = _mapper.Map<Announcement>(request);

            await _announcementBusinessRules.AnnouncementShouldNotExistsWhenInsert(announcement.Name);

            await _announcementRepository.AddAsync(announcement);

            CreatedAnnouncementResponse response = _mapper.Map<CreatedAnnouncementResponse>(announcement);
            return response;
        }
    }
}