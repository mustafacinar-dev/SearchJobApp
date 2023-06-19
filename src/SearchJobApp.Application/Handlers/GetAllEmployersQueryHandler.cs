using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetAllEmployersQueryHandler : IRequestHandler<GetAllEmployersQuery, IEnumerable<EmployerDto>>
{
    private readonly IEmployerElasticSearchService _employerElasticSearchService;
    private readonly IMapper _mapper;

    public GetAllEmployersQueryHandler(IEmployerElasticSearchService employerElasticSearchService, IMapper mapper)
    {
        _employerElasticSearchService = employerElasticSearchService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployerDto>> Handle(GetAllEmployersQuery request,
        CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<EmployerDto>>(await _employerElasticSearchService.GetAllAsync());
    }
}