using System;

namespace MortgageCalculator.Models;

public class SavedCalculation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Name { get; set; } = "未命名試算";
    public LoanInput LoanInput { get; set; } = new();
}
