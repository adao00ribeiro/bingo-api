using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace bingo_api.src.PolicyRequirements;

public class HorarioComercialRequirement : IAuthorizationRequirement
{
    public HorarioComercialRequirement() { }
}
