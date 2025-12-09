using MortgageCalculator.Models;

namespace MortgageCalculator.Services;

public interface IStorageService
{
    Task SaveCalculationAsync(SavedCalculation calculation);
    Task<List<SavedCalculation>> GetHistoryAsync();
    Task DeleteCalculationAsync(Guid id);
    Task ClearHistoryAsync();
}
