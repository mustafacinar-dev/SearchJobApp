using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Exceptions;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetEmployerWithPostsQueryHandler : IRequestHandler<GetEmployerWithPostsQuery, EmployerWithPostsDto>
{
    private readonly IEmployerElasticSearchService _employerElasticSearchService;
    private readonly IPostElasticSearchService _postElasticSearchService;
    private readonly IMapper _mapper;

    public GetEmployerWithPostsQueryHandler(IEmployerElasticSearchService employerElasticSearchService, 
        IPostElasticSearchService postElasticSearchService,
        IMapper mapper)
    {
        _employerElasticSearchService = employerElasticSearchService;
        _postElasticSearchService = postElasticSearchService;
        _mapper = mapper;
    }

    public async Task<EmployerWithPostsDto> Handle(GetEmployerWithPostsQuery request,
        CancellationToken cancellationToken)
    {
        var employer = _mapper.Map<EmployerWithPostsDto>(await _employerElasticSearchService.GetAsync(request.EmployerId));
        if (employer == null)
        {
            throw new EmployerNotFoundException();
        }
        
        employer.Posts = _mapper.Map<IEnumerable<PostDto>>(await _postElasticSearchService.GetPostsByEmployerId(employer.Id.ToString()));
        return employer;
    }
}