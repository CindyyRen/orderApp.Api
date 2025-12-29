using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Auth.Commands;

public record LoginCommand : IRequest<AuthResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}