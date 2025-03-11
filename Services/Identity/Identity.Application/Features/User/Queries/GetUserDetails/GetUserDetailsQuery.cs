using Identity.Application.Dtos;
using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.User.Queries.GetUserDetails
{
    public record GetUserDetailsQuery(Guid? Id) : IRequest<Result<UserDto>>;
}
