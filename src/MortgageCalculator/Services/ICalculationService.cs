using MortgageCalculator.Models;

namespace MortgageCalculator.Services;

public interface ICalculationService
{
    LoanResult CalculateLoan(LoanInput input);
    AffordabilityResult CalculateAffordability(AffordabilityInput input);
}
