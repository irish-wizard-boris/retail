using System;

namespace Abysalto.Retail.Modules.Cart.Domain.Entities;

public class CartDomainException : Exception
{
    public CartDomainException() { }
    public CartDomainException(string message) : base(message) { }
    public CartDomainException(string message, Exception inner) : base(message, inner) { }
}