using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DTOs;
using FluentValidation;

namespace Common.Validations
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.OrderDate).GreaterThan(DateTime.MinValue).WithMessage("Order date must be valid.");
        }
    }

    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product ID must be greater than zero.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }

}


