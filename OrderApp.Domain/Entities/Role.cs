using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities;

public enum RoleType
{
    Admin,
    User,
    Manager,
    Guest
}

public class Role : IdentityRole<Guid>
{
    public RoleType RoleType { get; set; }
    public string Description { get; set; } = string.Empty; // 可选，业务用途
}
