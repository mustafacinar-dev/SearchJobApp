using FluentValidation;
using MediatR;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.Exceptions;
using SearchJobApp.Application.Helpers;
using SearchJobApp.Application.Interfaces.Repositories;
using SearchJobApp.Application.Interfaces.Services;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Handlers;

public class CreateEmployerCommandHandler : IRequestHandler<CreateEmployerCommand, Guid>
{
    private readonly IEmployerRepository _employerRepository;
    private readonly IEmployerElasticSearchService _employerElasticSearchService;
    private readonly IValidator<CreateEmployerCommand> _validator;

    public CreateEmployerCommandHandler(IEmployerRepository employerRepository,
        IEmployerElasticSearchService employerElasticSearchService,
        IValidator<CreateEmployerCommand> validator)
    {
        _employerRepository = employerRepository;
        _employerElasticSearchService = employerElasticSearchService;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateEmployerCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Employer is not valid", validationResult.Errors, true);
        }

        var employer = await _employerRepository.GetEmployerByPhoneOrEmail(request.Phone, request.Email);
        if (employer != null)
        {
            if (employer.Email == request.Email)
                throw new EmailAddressUsedBeforeException();

            throw new NumberUsedBeforeException();
        }

        try
        {
            employer = await _employerRepository.AddAsync(new Employer
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Password = PasswordHasher.HashPassword(request.Password),
                Address = request.Address,
                Phone = request.Phone,
                Title = request.Title,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                RemainingPostingQuantity = 2
            });

            await _employerElasticSearchService.InsertAsync(employer);

            return employer.Id;
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message, exception);
        }
    }
}