using AutoMapper;
using MediatR;
using SearchJobApp.Application.DTO;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Application.Queries;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Handlers;

public class GetPostsByEmployerIdQueryHandler : IRequestHandler<GetPostsByEmployerIdQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public GetPostsByEmployerIdQueryHandler(IPostRepository postRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PostDto>> Handle(GetPostsByEmployerIdQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<PostDto>>(await _postRepository.FindAsync(p => p.EmployerId == request.EmployerId));
    }
}