using MediatR;
using SearchJobApp.Application.DTO;

namespace SearchJobApp.Application.Queries;

public class GetAllEmployersQuery : IRequest<IEnumerable<EmployerDto>>
{
}