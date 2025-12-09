namespace MortgageCalculator.Models;

public class LoanResult
{
    public decimal MonthlyPayment { get; set; } // For PrincipalAndInterest, this is the first month's payment or average
    public decimal TotalPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public List<AmortizationScheduleItem> Schedule { get; set; } = new();
}
