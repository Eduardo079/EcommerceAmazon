using Ecommerce.Application.Specifications.Products;

namespace Ecommerce.Application.Specifications;

// Clase base para parámetros de especificación (filtros/orden/paginación) que otras clases heredarán.
public abstract class SpecificationParams
{
    public string? Sort { get; set; }   // Criterio de ordenamiento (ej.: "priceAsc", "nameDesc", etc.). Opcional.

    public int PageIndex { get; set; } = 1;// Número de página a solicitar. Por defecto inicia en 1.

    private const int MaxPageSize = 50;// Límite superior permitido para el tamaño de página.

    private int _PageSize = 3;// Tamaño de página por defecto (cuántos registros por página).

    public int PageSize
    {
        get => _PageSize;// Devuelve el tamaño de página actual.

        // Asigna el tamaño de página solicitado; si excede el máximo, usa MaxPageSize.
        // Nota: si 'value' es menor o igual a MaxPageSize, se asigna tal cual (podría ser 0 si así se envía).
        set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    // Texto de búsqueda libre para filtrar resultados. Opcional.
    public string? Search { get; set; }

    public static implicit operator SpecificationParams(ProductSpecification v)
    {
        throw new NotImplementedException();
    }
}