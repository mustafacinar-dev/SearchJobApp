using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetSearchQueryHandler : IRequestHandler<GetSearchQuery, IEnumerable<PostDto>>
{
    private readonly IPostElasticSearchService _postElasticSearchService;
    private readonly IMapper _mapper;

    public GetSearchQueryHandler(IPostElasticSearchService postElasticSearchService, IMapper mapper)
    {
        _postElasticSearchService = postElasticSearchService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetSearchQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<PostDto>>(await _postElasticSearchService.SearchAsync(request));
    }
}