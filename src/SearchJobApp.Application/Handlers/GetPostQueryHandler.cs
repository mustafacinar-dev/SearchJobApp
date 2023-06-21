using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Application.Queries;

namespace SearchJobApp.Application.Handlers;

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDto>
{
    private readonly IPostElasticSearchService _postElasticSearchService;
    private readonly IMapper _mapper;

    public GetPostQueryHandler(IPostElasticSearchService postElasticSearchService, IMapper mapper)
    {
        _postElasticSearchService = postElasticSearchService;
        _mapper = mapper;
    }

    public async Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken) =>
        _mapper.Map<PostDto>(await _postElasticSearchService.GetAsync(request.PostId));
}