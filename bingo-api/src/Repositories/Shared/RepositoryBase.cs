using System.Linq.Expressions;
using bingo_api.src.Context;
using bingo_api.src.Entities.Shared;
using bingo_api.src.Interfaces.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace bingo_api.src.Repositories.Shared;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
{
    protected readonly DataContext Context;
    public RepositoryBase(DataContext dataContext) =>
    Context = dataContext;
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int? pageNumber = null, int? pageSize = null,
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeProperties = null
    )
    {
        IQueryable<TEntity> query = BuildQueryWithIncludes(includeProperties);
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }
        var entities = await query.ToListAsync();
        // Converte as datas de UTC para o horário local
        foreach (var entity in entities)
        {
            ConvertDatesToLocal(entity);
        }
        return entities;
    }
  public virtual async Task<TEntity?> GetByIdAsync(
    Guid id,
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeProperties = null)
{
    IQueryable<TEntity> query = BuildQueryWithIncludes(includeProperties);
    // como estamos usando query, precisa buscar por Where

    Console.WriteLine(query);
    return await query.FirstOrDefaultAsync(e => e.Id == id);
}

    public virtual async Task<Guid> AddAsync(TEntity objeto)
    {
        Context.Add(objeto);
        await Context.SaveChangesAsync();
        return objeto.Id;
    }

    public virtual async Task UpdateAsync(TEntity objeto)
    {
        var objetUpdate = await GetByIdAsync(objeto.Id) ?? throw new Exception("O registro não existe na base de dados.");

        foreach (var prop in objeto.GetType().GetProperties())
        {
            var entityProp = typeof(TEntity).GetProperty(prop.Name);
            if (entityProp != null && entityProp.CanWrite)
            {
                entityProp.SetValue(objetUpdate, prop.GetValue(objeto));
            }
        }
        Context.Entry(objetUpdate).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
    public virtual async Task UpdatePartialAsync(Guid id, Dictionary<string, object?> updatedValues)
    {
        var existingEntity = await GetByIdAsync(id) ?? throw new Exception("O registro não existe na base de dados.");

        var entityType = typeof(TEntity);
        foreach (var entry in updatedValues)
        {
            var propertyInfo = entityType.GetProperty(entry.Key);
            if (propertyInfo == null || !propertyInfo.CanWrite)
                continue;

            var targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            object? convertedValue = entry.Value is null ? null : Convert.ChangeType(entry.Value, targetType);

            propertyInfo.SetValue(existingEntity, convertedValue);
        }

        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
    public virtual async Task RemoveAsync(TEntity objeto)
    {
        Context.Set<TEntity>().Remove(objeto);
        await Context.SaveChangesAsync();
    }

    public virtual async Task RemoveByIdAsync(Guid id)
    {
        var objeto = await GetByIdAsync(id) ?? throw new Exception("O registro não existe na base de dados.");

        await RemoveAsync(objeto);
    }
    private void ConvertDatesToLocal<TEntity>(TEntity entity) where TEntity : class
    {
        // Use reflexão para encontrar e converter propriedades DateTime
        var properties = entity.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(DateTime));

        foreach (var property in properties)
        {

            var value = (DateTime)property.GetValue(entity);
            // Converte o valor para o horário local
            var localValue = TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.Local);
            property.SetValue(entity, localValue);
        }
    }
    protected IQueryable<TEntity> BuildQueryWithIncludes(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeProperties)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

         if (includeProperties != null)
        query = includeProperties(query);

        return query;
    }
    public async Task<int> CountAsync()
    {
        return await Context.Set<TEntity>().CountAsync();
    }

    public void Dispose() =>
        Context.Dispose();


}