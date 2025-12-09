using System.ComponentModel.DataAnnotations;

namespace MortgageCalculator.Models;

public class LoanInput
{
    [Required(ErrorMessage = "請輸入貸款金額")]
    [Range(10000, 1000000000, ErrorMessage = "貸款金額必須在 1 萬到 10 億之間")]
    public decimal LoanAmount { get; set; } = 10000000;

    [Required(ErrorMessage = "請輸入年利率")]
    [Range(0.1, 20, ErrorMessage = "年利率必須在 0.1% 到 20% 之間")]
    public double AnnualInterestRate { get; set; } = 2.0;

    [Required(ErrorMessage = "請輸入貸款年限")]
    [Range(1, 50, ErrorMessage = "貸款年限必須在 1 到 50 年之間")]
    public int LoanTermYears { get; set; } = 30;

    [Required(ErrorMessage = "請選擇還款方式")]
    public RepaymentType RepaymentType { get; set; } = RepaymentType.PrincipalAndInterest;

    [Range(0, 60, ErrorMessage = "寬限期必須在 0 到 60 個月之間")]
    public int GracePeriodMonths { get; set; } = 0;

    // US2: Input Mode
    public decimal TotalPrice { get; set; } = 12500000; // Default total price
    public double LoanPercentage { get; set; } = 80; // Default 80%

    public List<RateStage> RateStages { get; set; } = new List<RateStage>();
}
