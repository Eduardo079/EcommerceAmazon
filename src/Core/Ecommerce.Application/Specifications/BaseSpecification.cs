using System.Linq.Expressions;

namespace Ecommerce.Application.Specifications;

/// <summary>
/// Implementación base de una especificación para el tipo genérico <typeparamref name="T"/>.
/// Contiene soporte para: criterio (WHERE), includes (carga ansiosa),
/// ordenamientos (asc/desc) y paginación (Skip/Take).
/// </summary>
/// <typeparam name="T">Tipo de entidad sobre el cual se aplicará la especificación.</typeparam>
public class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Constructor por defecto. Permite construir la especificación
    /// e ir configurándola luego (criterio, includes, orden, paginación).
    /// </summary>
    public BaseSpecification()
    {

    }

    /// <summary>
    /// Constructor que recibe un criterio principal de filtrado (WHERE).
    /// </summary>
    /// <param name="criteria">
    /// Expresión booleana que debe cumplirse para que un elemento de tipo <typeparamref name="T"/> 
    /// sea incluido en el resultado. Ej.: <c>x =&gt; x.Activo</c>.
    /// </param>
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <summary>
    /// Criterio principal (WHERE) de la especificación. Puede ser <c>null</c> si no se definió.
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Colección de expresiones de navegación a incluir (Eager Loading) durante la consulta.
    /// Cada expresión suele apuntar a una propiedad de navegación. Ej.: <c>x =&gt; x.Categoria</c>.
    /// </summary>
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, Object>>>();

    /// <summary>
    /// Expresión que define la propiedad por la cual se ordenará ascendentemente (ORDER BY).
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <summary>
    /// Expresión que define la propiedad por la cual se ordenará descendentemente (ORDER BY ... DESC).
    /// </summary>
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <summary>
    /// Cantidad de elementos a tomar (LIMIT/TOP) al aplicar paginación.
    /// </summary>
    public int Take { get; private set; }

    /// <summary>
    /// Cantidad de elementos a omitir (OFFSET) al aplicar paginación.
    /// </summary>
    public int Skip { get; private set; }

    /// <summary>
    /// Indica si la paginación está habilitada (si deben aplicarse <see cref="Skip"/> y <see cref="Take"/>).
    /// </summary>
    public bool IsPagingEnable { get; private set; }

    /// <summary>
    /// Define la expresión de orden ascendente (ORDER BY).
    /// </summary>
    /// <param name="ordeByExpression">Expresión sobre <typeparamref name="T"/> que selecciona la clave de orden.</param>
    protected void AddOrderBy(Expression<Func<T, object>> ordeByExpression)
    {
        OrderBy = ordeByExpression;
    }

    /// <summary>
    /// Define la expresión de orden descendente (ORDER BY ... DESC).
    /// </summary>
    /// <param name="ordeByExpression">Expresión sobre <typeparamref name="T"/> que selecciona la clave de orden.</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> ordeByExpression)
    {
        OrderByDescending = ordeByExpression;
    }

    /// <summary>
    /// Habilita y configura la paginación estableciendo <see cref="Skip"/> y <see cref="Take"/>.
    /// </summary>
    /// <param name="skip">Cantidad de elementos a omitir (OFFSET).</param>
    /// <param name="take">Cantidad de elementos a tomar (LIMIT/TOP).</param>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnable = true;
    }
    /// <summary>
    /// Agrega una expresión de navegación a la colección de <see cref="Includes"/> para Eager Loading.
    /// </summary>
    /// <param name="includeExpression">
    /// Expresión que apunta a una propiedad de navegación de <typeparamref name="T"/>.
    /// </param>
    protected void AddInclude(Expression<Func<T, Object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

}