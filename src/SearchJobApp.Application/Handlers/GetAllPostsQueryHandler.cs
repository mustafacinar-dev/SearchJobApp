using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostElasticSearchService _postElasticSearchService;
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IPostElasticSearchService postElasticSearchService, IMapper mapper)
    {
        _postElasticSearchService = postElasticSearchService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<PostDto>>(await _postElasticSearchService.GetAllAsync());
    }
}