using Ecommerce.Application.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Ecommerce.Infrastructure.Specification;
/// <summary>
    /// Aplica una <see cref="ISpecification{T}"/> sobre un <see cref="IQueryable{T}"/>:
    /// - Filtro (Criteria)
    /// - Orden (OrderBy / OrderByDescending)
    /// - Paginación (Skip/Take)
    /// - Includes (Eager Loading)
    /// Retorna el IQueryable resultante sin ejecutar la consulta (deferred execution).
    /// </summary>
    /// <typeparam name="T">Entidad raíz de la consulta.</typeparam>
public class SpecificationEvaluator<T> where T : class
{
    /// <summary>
        /// Construye el IQueryable aplicando las reglas de la especificación.
        /// No ejecuta la consulta; solo compone el árbol de expresión.
        /// </summary>
        /// <param name="inputQuery">Origen (DbSet/consulta base) sobre el que se aplican las reglas.</param>
        /// <param name="spec">Especificación con filtros, includes, orden y paginación.</param>
        /// <returns>IQueryable con todas las transformaciones encadenadas.</returns>
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        // Si hay criterio (WHERE), aplícalo.
        if (spec.Criteria != null)
        {
            inputQuery = inputQuery.Where(spec.Criteria);
        }

        // Si hay orden ascendente (ORDER BY), aplícalo.
        if (spec.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(spec.OrderBy);
        }

        // Si hay orden descendente (ORDER BY ... DESC), aplícalo.
        // Nota: aquí se llama a OrderBy(..) pasando OrderByDescending; normalmente sería OrderByDescending(..).
        if (spec.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderBy(spec.OrderByDescending);
        }

        // Si la paginación está habilitada, aplica Skip y Take.
        if (spec.IsPagingEnable)
        {
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
        }

        // Aplica Includes (Eager Loading) acumulándolos sobre la consulta.
        // ¡Ojo!: spec.Includes se asume no nulo por el operador !; si fuera null, lanzará excepción.
        inputQuery = spec.Includes!.Aggregate(inputQuery, (current, include)
        => current.Include(include))
        .AsSingleQuery() // Fuerza a EF Core a ejecutar como una sola consulta (en vez de SplitQuery).
        .AsNoTracking(); // Deshabilita el cambio de seguimiento para mejorar performance en lecturas.


        // Devuelve el IQueryable ya compuesto (aún no ejecutado).
        return inputQuery;
    }
}