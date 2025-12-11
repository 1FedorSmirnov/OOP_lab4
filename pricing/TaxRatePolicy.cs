using Lab4FoodDelivery.domain;

namespace Lab4FoodDelivery.pricing;

public class TaxRatePolicy : ITaxPolicy
{
    private readonly decimal _taxRate;
    
    public TaxRatePolicy(decimal taxRate)
    {
        if (taxRate <= 0)
            throw new ArgumentException("Tax rate must be greater than zero", nameof(taxRate));
        _taxRate = taxRate;
    }
    public decimal ApplyTax(decimal sumAfterDiscount, Order order)
    {
        return sumAfterDiscount + sumAfterDiscount*_taxRate;
    }
}