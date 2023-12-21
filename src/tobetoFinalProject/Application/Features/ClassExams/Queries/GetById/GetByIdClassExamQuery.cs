using Application.Features.ClassExams.Constants;
using Application.Features.ClassExams.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.ClassExams.Constants.ClassExamsOperationClaims;

namespace Application.Features.ClassExams.Queries.GetById;

public class GetByIdClassExamQuery : IRequest<GetByIdClassExamResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => new[] { Admin, Read };

    public class GetByIdClassExamQueryHandler : IRequestHandler<GetByIdClassExamQuery, GetByIdClassExamResponse>
    {
        private readonly IMapper _mapper;
        private readonly IClassExamRepository _classExamRepository;
        private readonly ClassExamBusinessRules _classExamBusinessRules;

        public GetByIdClassExamQueryHandler(IMapper mapper, IClassExamRepository classExamRepository, ClassExamBusinessRules classExamBusinessRules)
        {
            _mapper = mapper;
            _classExamRepository = classExamRepository;
            _classExamBusinessRules = classExamBusinessRules;
        }

        public async Task<GetByIdClassExamResponse> Handle(GetByIdClassExamQuery request, CancellationToken cancellationToken)
        {
            ClassExam? classExam = await _classExamRepository.GetAsync(predicate: ce => ce.Id == request.Id, cancellationToken: cancellationToken);
            await _classExamBusinessRules.ClassExamShouldExistWhenSelected(classExam);

            GetByIdClassExamResponse response = _mapper.Map<GetByIdClassExamResponse>(classExam);
            return response;
        }
    }
}