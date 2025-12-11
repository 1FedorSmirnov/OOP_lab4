using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class NoTaxPolicy : ITaxPolicy
{
    public decimal ApplyTax(decimal sumAfterDiscount, Order order)
    {
        return sumAfterDiscount;
    }
}