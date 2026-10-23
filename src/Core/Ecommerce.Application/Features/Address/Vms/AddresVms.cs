using AutoMapper.Configuration.Conventions;

namespace Ecommerce.Application.Features.Addresses.Vms;

public class AddressVms
{
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Departamento { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Pais { get; set; }
}