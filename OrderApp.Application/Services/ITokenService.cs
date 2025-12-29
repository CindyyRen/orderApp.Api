using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);
}