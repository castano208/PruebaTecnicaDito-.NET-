using FluentValidation;

namespace BackendAPI.Application.Features.Productos.Commands.CreateProducto;

public class CreateProductoCommandValidator : AbstractValidator<CreateProductoCommand>
{
    public CreateProductoCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres.");

        RuleFor(x => x.Categoria)
            .MaximumLength(100).WithMessage("La categoría no puede exceder 100 caracteres.");

        RuleFor(x => x.Codigo)
            .MaximumLength(50).WithMessage("El código no puede exceder 50 caracteres.");

        RuleFor(x => x.ImagenUrl)
            .MaximumLength(500).WithMessage("La URL de imagen no puede exceder 500 caracteres.");
    }
}
