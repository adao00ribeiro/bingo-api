using bingo_api.src.Entities;
using bingo_api.src.Interfaces.Repositories.Shared;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Interfaces.Repositories;
public interface IPunterRepository : IRepositoryBase<Punter>
{
    Task<Punter> GetByEmailAsync(string email);
}