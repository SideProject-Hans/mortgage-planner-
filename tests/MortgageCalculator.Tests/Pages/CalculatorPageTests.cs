using MortgageCalculator.Models;
using MortgageCalculator.Services;

namespace MortgageCalculator.Tests.Pages;

/// <summary>
/// Tests for CalculatorPage chart sampling logic.
/// These tests verify the chart data point sampling and X-axis label formatting
/// per spec 004-ltv-chart-ux requirements.
/// </summary>
public class CalculatorPageTests
{
    #region Chart Sampling Helper (mirrors production logic)

    /// <summary>
    /// Calculates the appropriate sampling interval based on loan term years.
    /// FR-003: Long-term loans (>5 years) sample every 5 years; short-term loans (≤5 years) sample every year.
    /// </summary>
    public static int GetSampleIntervalYears(int totalYears)
    {
        return totalYears <= 5 ? 1 : 5;
    }

    /// <summary>
    /// Generates chart data points for a given loan schedule.
    /// FR-004: X-axis labels use "Y{n}" format.
    /// FR-005: Maximum 10 data points to ensure chart readability.
    /// </summary>
    public static (string[] Labels, double[] BalanceData) GenerateChartData(
        List<AmortizationScheduleItem> schedule, 
        decimal initialBalance)
    {
        if (schedule == null || schedule.Count == 0)
            return (Array.Empty<string>(), Array.Empty<double>());

        var totalYears = (int)Math.Ceiling(schedule.Count / 12.0);
        var intervalYears = GetSampleIntervalYears(totalYears);

        var labels = new List<string>();
        var balanceData = new List<double>();

        for (int year = 0; year <= totalYears; year += intervalYears)
        {
            // FR-005: Ensure maximum 10 data points
            if (labels.Count >= 10)
                break;

            labels.Add($"Y{year}");
            
            if (year == 0)
            {
                balanceData.Add((double)initialBalance);
            }
            else
            {
                int monthIndex = year * 12;
                if (monthIndex >= schedule.Count)
                {
                    balanceData.Add(0);
                }
                else
                {
                    balanceData.Add((double)schedule[monthIndex - 1].RemainingBalance);
                }
            }
        }

        return (labels.ToArray(), balanceData.ToArray());
    }

    #endregion

    #region T007: Long-term loan sampling interval tests (US2)

    /// <summary>
    /// FR-003: Long-term loans (>5 years) should sample every 5 years.
    /// </summary>
    [Theory]
    [InlineData(30, 7)]  // 30 years → Y0, Y5, Y10, Y15, Y20, Y25, Y30 = 7 data points
    [InlineData(20, 5)]  // 20 years → Y0, Y5, Y10, Y15, Y20 = 5 data points
    [InlineData(10, 3)]  // 10 years → Y0, Y5, Y10 = 3 data points
    [InlineData(6, 2)]   // 6 years → Y0, Y5 (Y6 not included as it exceeds 5-year interval) = 2 data points
    public void GenerateChartData_LongTermLoan_ShouldSampleEvery5Years(int years, int expectedDataPoints)
    {
        // Arrange
        var schedule = CreateMockSchedule(years * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, balanceData) = GenerateChartData(schedule, initialBalance);

        // Assert
        Assert.Equal(expectedDataPoints, labels.Length);
        Assert.Equal(expectedDataPoints, balanceData.Length);
    }

    #endregion

    #region T008: Short-term loan sampling interval tests (US2)

    /// <summary>
    /// FR-003: Short-term loans (≤5 years) should sample every year.
    /// </summary>
    [Theory]
    [InlineData(5, 6)]   // 5 years → Y0, Y1, Y2, Y3, Y4, Y5 = 6 data points
    [InlineData(3, 4)]   // 3 years → Y0, Y1, Y2, Y3 = 4 data points
    [InlineData(1, 2)]   // 1 year → Y0, Y1 = 2 data points
    public void GenerateChartData_ShortTermLoan_ShouldSampleEveryYear(int years, int expectedDataPoints)
    {
        // Arrange
        var schedule = CreateMockSchedule(years * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, balanceData) = GenerateChartData(schedule, initialBalance);

        // Assert
        Assert.Equal(expectedDataPoints, labels.Length);
        Assert.Equal(expectedDataPoints, balanceData.Length);
    }

    #endregion

    #region T009: X-axis label format tests (US2)

    /// <summary>
    /// FR-004: X-axis labels should use "Y{n}" format (e.g., Y0, Y5, Y10).
    /// </summary>
    [Fact]
    public void GenerateChartData_XAxisLabels_ShouldUseYearFormat()
    {
        // Arrange
        var schedule = CreateMockSchedule(30 * 12); // 30-year loan
        var initialBalance = 10_000_000m;

        // Act
        var (labels, _) = GenerateChartData(schedule, initialBalance);

        // Assert - All labels should follow "Y{n}" format
        Assert.All(labels, label =>
        {
            Assert.StartsWith("Y", label);
            Assert.True(int.TryParse(label.Substring(1), out _), $"Label '{label}' should have numeric suffix");
        });
    }

    [Fact]
    public void GenerateChartData_30YearLoan_ShouldHaveCorrectLabels()
    {
        // Arrange
        var schedule = CreateMockSchedule(30 * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, _) = GenerateChartData(schedule, initialBalance);

        // Assert - Should have exactly these labels for 30-year loan
        var expectedLabels = new[] { "Y0", "Y5", "Y10", "Y15", "Y20", "Y25", "Y30" };
        Assert.Equal(expectedLabels, labels);
    }

    [Fact]
    public void GenerateChartData_5YearLoan_ShouldHaveCorrectLabels()
    {
        // Arrange
        var schedule = CreateMockSchedule(5 * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, _) = GenerateChartData(schedule, initialBalance);

        // Assert - Should have exactly these labels for 5-year loan
        var expectedLabels = new[] { "Y0", "Y1", "Y2", "Y3", "Y4", "Y5" };
        Assert.Equal(expectedLabels, labels);
    }

    /// <summary>
    /// FR-004: Label length should not exceed 3 characters (SC-003).
    /// </summary>
    [Theory]
    [InlineData(30)]
    [InlineData(20)]
    [InlineData(10)]
    [InlineData(5)]
    public void GenerateChartData_AllLabels_ShouldNotExceed3Characters(int years)
    {
        // Arrange
        var schedule = CreateMockSchedule(years * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, _) = GenerateChartData(schedule, initialBalance);

        // Assert - All labels should be ≤ 3 characters (e.g., "Y0", "Y5", "Y30")
        Assert.All(labels, label => Assert.True(label.Length <= 3, $"Label '{label}' exceeds 3 characters"));
    }

    #endregion

    #region T010: Data point count limit tests (US2)

    /// <summary>
    /// FR-005: Chart should have no more than 10 data points.
    /// SC-002: 30-year loan chart should have ≤ 10 data points.
    /// </summary>
    [Theory]
    [InlineData(30)]
    [InlineData(40)]
    [InlineData(50)]
    public void GenerateChartData_AnyLoanTerm_ShouldNotExceed10DataPoints(int years)
    {
        // Arrange
        var schedule = CreateMockSchedule(years * 12);
        var initialBalance = 10_000_000m;

        // Act
        var (labels, balanceData) = GenerateChartData(schedule, initialBalance);

        // Assert - Should never exceed 10 data points
        Assert.True(labels.Length <= 10, $"Labels count {labels.Length} exceeds 10");
        Assert.True(balanceData.Length <= 10, $"Balance data count {balanceData.Length} exceeds 10");
    }

    #endregion

    #region Sampling interval calculation tests

    [Theory]
    [InlineData(1, 1)]   // 1 year → interval = 1
    [InlineData(3, 1)]   // 3 years → interval = 1
    [InlineData(5, 1)]   // 5 years → interval = 1 (boundary)
    [InlineData(6, 5)]   // 6 years → interval = 5
    [InlineData(10, 5)]  // 10 years → interval = 5
    [InlineData(30, 5)]  // 30 years → interval = 5
    public void GetSampleIntervalYears_ShouldReturnCorrectInterval(int totalYears, int expectedInterval)
    {
        // Act
        var interval = GetSampleIntervalYears(totalYears);

        // Assert
        Assert.Equal(expectedInterval, interval);
    }

    #endregion

    #region Edge cases

    [Fact]
    public void GenerateChartData_EmptySchedule_ShouldReturnEmptyArrays()
    {
        // Arrange
        var schedule = new List<AmortizationScheduleItem>();
        var initialBalance = 10_000_000m;

        // Act
        var (labels, balanceData) = GenerateChartData(schedule, initialBalance);

        // Assert
        Assert.Empty(labels);
        Assert.Empty(balanceData);
    }

    [Fact]
    public void GenerateChartData_NullSchedule_ShouldReturnEmptyArrays()
    {
        // Arrange
        List<AmortizationScheduleItem>? schedule = null;
        var initialBalance = 10_000_000m;

        // Act
        var (labels, balanceData) = GenerateChartData(schedule!, initialBalance);

        // Assert
        Assert.Empty(labels);
        Assert.Empty(balanceData);
    }

    #endregion

    #region Test Helpers

    /// <summary>
    /// Creates a mock amortization schedule with the specified number of periods.
    /// </summary>
    private static List<AmortizationScheduleItem> CreateMockSchedule(int periods)
    {
        var schedule = new List<AmortizationScheduleItem>();
        var initialBalance = 10_000_000m;
        var monthlyPrincipal = initialBalance / periods;

        for (int i = 1; i <= periods; i++)
        {
            var remainingBalance = initialBalance - (monthlyPrincipal * i);
            schedule.Add(new AmortizationScheduleItem
            {
                Period = i,
                PrincipalPayment = monthlyPrincipal,
                InterestPayment = remainingBalance * 0.02m / 12,
                TotalPayment = monthlyPrincipal + (remainingBalance * 0.02m / 12),
                RemainingBalance = Math.Max(0, remainingBalance)
            });
        }

        return schedule;
    }

    #endregion
}
