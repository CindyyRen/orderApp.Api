using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Auth.Commands;

// Application/Auth/Commands/RegisterCommand.cs
public record RegisterCommand : IRequest<AuthResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FullName { get; init; }
}

public record AuthResponse(string Token, string Email, List<string> Roles);