using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

public static class ModelBuilderExtensions
{
    public static void ApplyAllConfigurationsFromCurrentAssembly(this ModelBuilder modelBuilder, string @namespace)
    {
        var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
                     && m.GetParameters().Length == 1
                     && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(c => c.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                        && c.Namespace == @namespace)
            .ToList();

        foreach (var type in types)
        {
            var entityType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)).GenericTypeArguments[0];
            var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(entityType);
            applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
        }
    }
}
