using FluentValidation;
using Purchasing.Domain.Enums;

namespace Purchasing.Application.Orders.Commands.UpdateStatusOrder;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus NewStatus)
    : ICommand<UpdateOrderStatusResult>;

public record UpdateOrderStatusResult(bool IsSuccess);

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
        RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Valid status is required");
    }
}