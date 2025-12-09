namespace MortgageCalculator.Models;

public class AmortizationScheduleItem
{
    public int Period { get; set; }
    public decimal PrincipalPayment { get; set; }
    public decimal InterestPayment { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RemainingBalance { get; set; }
}
