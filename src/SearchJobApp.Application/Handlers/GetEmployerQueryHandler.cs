using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetEmployerQueryHandler : IRequestHandler<GetEmployerQuery, EmployerDto>
{
    private readonly IEmployerElasticSearchService _employerElasticSearchService;
    private readonly IMapper _mapper;

    public GetEmployerQueryHandler(IEmployerElasticSearchService employerElasticSearchService,
        IMapper mapper)
    {
        _employerElasticSearchService = employerElasticSearchService;
        _mapper = mapper;
    }

    public async Task<EmployerDto> Handle(GetEmployerQuery request, CancellationToken cancellationToken) =>
        _mapper.Map<EmployerDto>(await _employerElasticSearchService.GetAsync(request.EmployerId));
}