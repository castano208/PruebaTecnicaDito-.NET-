using FluentValidation;
using BackendAPI.Domain.DTOs;

namespace BackendAPI.Application.Features.Pedidos.Commands.CreatePedido;

public class CreatePedidoCommandValidator : AbstractValidator<CreatePedidoCommand>
{
    public CreatePedidoCommandValidator()
    {
        RuleFor(x => x.UsuarioId)
            .GreaterThan(0).WithMessage("El ID del usuario debe ser mayor a 0.");

        RuleFor(x => x.PedidoItems)
            .NotEmpty().WithMessage("El pedido debe tener al menos un item.");

        RuleFor(x => x.Comentarios)
            .MaximumLength(500).WithMessage("Los comentarios no pueden exceder 500 caracteres.");

        RuleFor(x => x.DireccionEntrega)
            .MaximumLength(200).WithMessage("La dirección de entrega no puede exceder 200 caracteres.");

        RuleForEach(x => x.PedidoItems)
            .SetValidator(new CreatePedidoItemDtoValidator());
    }
}

public class CreatePedidoItemDtoValidator : AbstractValidator<CreatePedidoItemDto>
{
    public CreatePedidoItemDtoValidator()
    {
        RuleFor(x => x.ProductoId)
            .GreaterThan(0).WithMessage("El ID del producto debe ser mayor a 0.");

        RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
    }
}
