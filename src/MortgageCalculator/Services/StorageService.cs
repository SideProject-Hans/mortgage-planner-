using Blazored.LocalStorage;
using MortgageCalculator.Models;

namespace MortgageCalculator.Services;

public class StorageService : IStorageService
{
    private readonly ILocalStorageService _localStorage;
    private const string StorageKey = "mortgage_calculator_history";

    public StorageService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveCalculationAsync(SavedCalculation calculation)
    {
        var history = await GetHistoryAsync();
        history.Insert(0, calculation);
        
        // Limit history to 50 items
        if (history.Count > 50)
        {
            history = history.Take(50).ToList();
        }

        await _localStorage.SetItemAsync(StorageKey, history);
    }

    public async Task<List<SavedCalculation>> GetHistoryAsync()
    {
        return await _localStorage.GetItemAsync<List<SavedCalculation>>(StorageKey) 
               ?? new List<SavedCalculation>();
    }

    public async Task DeleteCalculationAsync(Guid id)
    {
        var history = await GetHistoryAsync();
        var item = history.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            history.Remove(item);
            await _localStorage.SetItemAsync(StorageKey, history);
        }
    }

    public async Task ClearHistoryAsync()
    {
        await _localStorage.RemoveItemAsync(StorageKey);
    }
}
