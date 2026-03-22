using System.Linq.Expressions;

namespace Ecommerce.Application.Specifications;

/// <summary>
    /// Contrato genérico de una especificación que describe
    /// filtros, inclusiones de navegación, ordenamientos y paginación
    /// para consultar una entidad del dominio.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo de la entidad sobre la que se aplicará la especificación
    /// (por ejemplo, Product, Customer, Order).
    /// </typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Expresión que define el criterio principal de filtrado (WHERE).
    /// Devuelve true para los elementos que deben incluirse en el resultado.
    /// Ejemplo: <c>x =&gt; x.Activo &amp;&amp; x.Precio &gt; 10</c>.
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Colección de expresiones que indican rutas de navegación a incluir
    /// (Eager Loading) en la consulta. Ejemplo: <c>x =&gt; x.Categoria</c>.
    /// </summary>
    List<Expression<Func<T, object>>>? Includes { get; }

    /// <summary>
    /// Expresión que indica la propiedad por la cual se ordenará
    /// ascendentemente (ORDER BY). Ejemplo: <c>x =&gt; x.Nombre</c>.
    /// </summary>
    Expression<Func<T, Object>>? OrderBy { get; }

    /// <summary>
    /// Expresión que indica la propiedad por la cual se ordenará
    /// descendentemente (ORDER BY ... DESC). Ejemplo: <c>x =&gt; x.FechaCreacion</c>.
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    // Límite de registros a tomar (TOP / LIMIT).
    int Take { get; }

    // Cantidad de registros a saltar (OFFSET) para paginación.
    int Skip { get; }

    // Indica si la paginación (Skip/Take) está habilitada.
    bool IsPagingEnable { get; }
}