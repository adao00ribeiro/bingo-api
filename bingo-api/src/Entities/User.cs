using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bingo_api.src.Entities;

public class User : IdentityUser
{
    public Guid EntityId { get; set; } // ID do Punter ou Seller
    public string EntityType { get; set; } // Nome da classe (ex: "Punter" ou "Seller")
}
