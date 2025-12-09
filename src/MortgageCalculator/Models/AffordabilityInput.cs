using System.ComponentModel.DataAnnotations;

namespace MortgageCalculator.Models;

public class AffordabilityInput
{
    [Required(ErrorMessage = "請輸入月收入")]
    [Range(0, 100000000, ErrorMessage = "月收入必須大於 0")]
    public decimal MonthlyIncome { get; set; } = 50000;

    [Range(0, 100000000, ErrorMessage = "每月債務必須大於等於 0")]
    public decimal MonthlyDebt { get; set; } = 0;

    [Range(1, 40, ErrorMessage = "貸款年限必須在 1 到 40 年之間")]
    public int LoanTermYears { get; set; } = 30;

    [Range(0, 20, ErrorMessage = "年利率必須在 0% 到 20% 之間")]
    public double AnnualInterestRate { get; set; } = 2.0;

    [Range(10, 80, ErrorMessage = "最高負債比必須在 10% 到 80% 之間")]
    public double MaxDTI { get; set; } = 60;
}
