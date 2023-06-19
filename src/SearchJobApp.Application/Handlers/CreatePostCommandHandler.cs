using FluentValidation;
using MediatR;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.Exceptions;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Handlers;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IEmployerRepository _employerRepository;
    private readonly IPostRepository _postRepository;
    private readonly IEmployerElasticSearchService _employerElasticSearchService;
    private readonly IPostElasticSearchService _postElasticSearchService;
    private readonly IValidator<CreatePostCommand> _validator;

    public CreatePostCommandHandler(IEmployerRepository employerRepository,
        IPostRepository postRepository,
        IEmployerElasticSearchService employerElasticSearchService,
        IPostElasticSearchService postElasticSearchService,
        IValidator<CreatePostCommand> validator)
    {
        _employerRepository = employerRepository;
        _postRepository = postRepository;
        _employerElasticSearchService = employerElasticSearchService;
        _postElasticSearchService = postElasticSearchService;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Employer is not valid", validationResult.Errors, true);
        }

        var employer = await _employerElasticSearchService.GetAsync(request.EmployerId);
        if (employer.RemainingPostingQuantity <= 0)
        {
            throw new RemainingPostingQuantityException();
        }

        try
        {
            var post = await _postRepository.AddAsync(new Post
            {
                Id = Guid.NewGuid(),
                EndDate = DateTime.Now.AddDays(15),
                StartDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Salary = request.Salary,
                WorkType = request.WorkType,
                PositionLevel = request.PositionLevel,
                Title = request.Title,
                Message = request.Message,
                AdditionalMessage = request.AdditionalMessage,
                QualityScore = CalculatePostQualityScore(request),
                EmployerId = request.EmployerId
            });

            post.EmployerTitle = employer.Title;
            await _postElasticSearchService.InsertAsync(post);

            employer.RemainingPostingQuantity -= 1;

            await _employerRepository.UpdateAsync(employer);
            await _employerElasticSearchService.UpdateAsync(employer.Id, employer);

            return post.Id;
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }

    private int CalculatePostQualityScore(CreatePostCommand request)
    {
        var qualityScore = 2;

        qualityScore += request.WorkType is null ? 0 : 1;
        qualityScore += string.IsNullOrWhiteSpace(request.Salary) ? 0 : 1;
        qualityScore += string.IsNullOrWhiteSpace(request.AdditionalMessage) ? 0 : 1;

        return qualityScore;
    }
}