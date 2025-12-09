using MortgageCalculator.Models;

namespace MortgageCalculator.Services;

public class CalculationService : ICalculationService
{
    public LoanResult CalculateLoan(LoanInput input)
    {
        var result = new LoanResult();
        
        if (input.RepaymentType == RepaymentType.PrincipalAndInterest)
        {
            CalculatePrincipalAndInterest(input, result);
        }
        else
        {
            CalculatePrincipalOnly(input, result);
        }

        return result;
    }

    private double GetMonthlyRate(LoanInput input, int month)
    {
        if (input.RateStages == null || !input.RateStages.Any())
        {
            return input.AnnualInterestRate / 12 / 100;
        }

        int currentMonthCount = 0;
        foreach (var stage in input.RateStages)
        {
            currentMonthCount += stage.DurationMonths;
            if (month <= currentMonthCount)
            {
                return (double)stage.InterestRate / 12 / 100;
            }
        }

        return (double)input.RateStages.Last().InterestRate / 12 / 100;
    }

    private void CalculatePrincipalOnly(LoanInput input, LoanResult result)
    {
        decimal loanAmount = input.LoanAmount;
        int totalMonths = input.LoanTermYears * 12;
        int gracePeriod = input.GracePeriodMonths;
        int repaymentMonths = totalMonths - gracePeriod;

        decimal monthlyPrincipal = 0;
        if (repaymentMonths > 0)
        {
            monthlyPrincipal = Math.Round(loanAmount / repaymentMonths, 0, MidpointRounding.AwayFromZero);
        }

        decimal remainingBalance = loanAmount;

        for (int i = 1; i <= totalMonths; i++)
        {
            double monthlyRate = GetMonthlyRate(input, i);
            decimal interestPayment = Math.Round(remainingBalance * (decimal)monthlyRate, 0, MidpointRounding.AwayFromZero);
            decimal principalPayment = 0;
            decimal currentTotalPayment = 0;

            if (i <= gracePeriod)
            {
                principalPayment = 0;
                currentTotalPayment = interestPayment;
            }
            else
            {
                principalPayment = monthlyPrincipal;
                
                if (remainingBalance - principalPayment < 0 || i == totalMonths)
                {
                    principalPayment = remainingBalance;
                }

                currentTotalPayment = principalPayment + interestPayment;
            }

            remainingBalance -= principalPayment;
            if (remainingBalance < 0) remainingBalance = 0;

            result.Schedule.Add(new AmortizationScheduleItem
            {
                Period = i,
                PrincipalPayment = principalPayment,
                InterestPayment = interestPayment,
                TotalPayment = currentTotalPayment,
                RemainingBalance = remainingBalance
            });
        }

        result.MonthlyPayment = result.Schedule.FirstOrDefault(x => x.Period > gracePeriod)?.TotalPayment ?? 0;
        result.TotalPayment = result.Schedule.Sum(x => x.TotalPayment);
        result.TotalInterest = result.Schedule.Sum(x => x.InterestPayment);
    }

    private void CalculatePrincipalAndInterest(LoanInput input, LoanResult result)
    {
        decimal loanAmount = input.LoanAmount;
        int totalMonths = input.LoanTermYears * 12;
        int gracePeriod = input.GracePeriodMonths;

        decimal remainingBalance = loanAmount;
        decimal monthlyPayment = 0;
        double currentMonthlyRate = -1; // Force recalc on first iteration

        for (int i = 1; i <= totalMonths; i++)
        {
            double newMonthlyRate = GetMonthlyRate(input, i);
            decimal interestPayment = Math.Round(remainingBalance * (decimal)newMonthlyRate, 0, MidpointRounding.AwayFromZero);
            decimal principalPayment = 0;
            decimal currentTotalPayment = 0;

            if (i <= gracePeriod)
            {
                principalPayment = 0;
                currentTotalPayment = interestPayment;
            }
            else
            {
                bool isFirstRepaymentMonth = (i == gracePeriod + 1);
                bool rateChanged = Math.Abs(newMonthlyRate - currentMonthlyRate) > 0.000000001;

                if (isFirstRepaymentMonth || rateChanged)
                {
                    int remainingMonths = totalMonths - i + 1;
                    if (newMonthlyRate > 0)
                    {
                        double ratePow = Math.Pow(1 + newMonthlyRate, remainingMonths);
                        decimal pmt = remainingBalance * (decimal)((newMonthlyRate * ratePow) / (ratePow - 1));
                        monthlyPayment = Math.Round(pmt, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        monthlyPayment = Math.Round(remainingBalance / remainingMonths, 0, MidpointRounding.AwayFromZero);
                    }
                }

                currentTotalPayment = monthlyPayment;
                principalPayment = currentTotalPayment - interestPayment;

                if (remainingBalance - principalPayment < 0 || i == totalMonths)
                {
                    principalPayment = remainingBalance;
                    currentTotalPayment = principalPayment + interestPayment;
                }
            }

            currentMonthlyRate = newMonthlyRate;
            remainingBalance -= principalPayment;
            if (remainingBalance < 0) remainingBalance = 0;

            result.Schedule.Add(new AmortizationScheduleItem
            {
                Period = i,
                PrincipalPayment = principalPayment,
                InterestPayment = interestPayment,
                TotalPayment = currentTotalPayment,
                RemainingBalance = remainingBalance
            });
        }

        result.MonthlyPayment = result.Schedule.FirstOrDefault(x => x.Period > gracePeriod)?.TotalPayment ?? 0;
        result.TotalPayment = result.Schedule.Sum(x => x.TotalPayment);
        result.TotalInterest = result.Schedule.Sum(x => x.InterestPayment);
    }

    public AffordabilityResult CalculateAffordability(AffordabilityInput input)
    {
        var result = new AffordabilityResult();
        
        // 1. Calculate Max Monthly Payment based on DTI
        // DTI = (Monthly Debt + Mortgage Payment) / Monthly Income
        // Max Mortgage Payment = (Max DTI * Monthly Income) - Monthly Debt
        
        decimal maxTotalMonthlyPayment = input.MonthlyIncome * ((decimal)input.MaxDTI / 100m);
        decimal maxMortgagePayment = maxTotalMonthlyPayment - input.MonthlyDebt;
        
        if (maxMortgagePayment < 0) maxMortgagePayment = 0;
        
        result.MaxMonthlyPayment = maxMortgagePayment;
        
        // 2. Calculate Max Loan Amount based on Max Mortgage Payment
        // Using standard amortization formula: P = (r * PV) / (1 - (1 + r)^-n)
        // PV = P * (1 - (1 + r)^-n) / r
        
        double monthlyRate = input.AnnualInterestRate / 12 / 100;
        int totalMonths = input.LoanTermYears * 12;
        
        if (monthlyRate > 0)
        {
            double ratePow = Math.Pow(1 + monthlyRate, -totalMonths);
            double pv = (double)maxMortgagePayment * (1 - ratePow) / monthlyRate;
            result.MaxLoanAmount = Math.Round((decimal)pv, 0, MidpointRounding.AwayFromZero);
        }
        else
        {
            result.MaxLoanAmount = maxMortgagePayment * totalMonths;
        }
        
        // 3. Generate Suggestions
        if (result.MaxLoanAmount < 1000000) // Example threshold
        {
            result.Suggestions.Add("您的可負擔貸款金額較低，建議增加頭期款或減少每月債務。");
        }
        
        if (input.MonthlyDebt > input.MonthlyIncome * 0.5m)
        {
            result.Suggestions.Add("您的每月債務佔比較高，可能會影響貸款審核。");
        }
        
        return result;
    }
}
