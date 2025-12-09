namespace MortgageCalculator.Models;

public class AffordabilityResult
{
    public decimal MaxMonthlyPayment { get; set; }
    public decimal MaxLoanAmount { get; set; }
    public List<string> Suggestions { get; set; } = new();
}
