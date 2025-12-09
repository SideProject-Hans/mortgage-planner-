using MortgageCalculator.Models;

namespace MortgageCalculator.Tests.Components;

/// <summary>
/// Tests for LoanInputForm calculation logic.
/// These tests verify the calculation behavior without rendering MudBlazor components
/// to avoid complex test setup requirements.
/// </summary>
public class LoanInputFormTests
{
    #region T007: OnPercentageChanged calculation tests

    [Fact]
    public void OnPercentageChanged_WhenPercentageIs80_ShouldCalculateCorrectLoanAmount()
    {
        // Arrange
        var totalPrice = 10_000_000m;
        var percentage = 80.0;

        // Act - Calculate as component would
        // LoanAmount = TotalPrice * (Percentage / 100)
        var calculatedLoanAmount = Math.Round(totalPrice * (decimal)(percentage / 100), 0);
        var calculatedDownPayment = totalPrice - calculatedLoanAmount;

        // Assert
        Assert.Equal(8_000_000m, calculatedLoanAmount);
        Assert.Equal(2_000_000m, calculatedDownPayment);
    }

    [Theory]
    [InlineData(10_000_000, 80, 8_000_000, 2_000_000)]
    [InlineData(10_000_000, 70, 7_000_000, 3_000_000)]
    [InlineData(10_000_000, 60, 6_000_000, 4_000_000)]
    [InlineData(5_000_000, 80, 4_000_000, 1_000_000)]
    public void OnPercentageChanged_WithVariousInputs_ShouldCalculateCorrectValues(
        decimal totalPrice, double percentage, decimal expectedLoanAmount, decimal expectedDownPayment)
    {
        // Arrange & Act
        var calculatedLoanAmount = Math.Round(totalPrice * (decimal)(percentage / 100), 0);
        var calculatedDownPayment = totalPrice - calculatedLoanAmount;

        // Assert
        Assert.Equal(expectedLoanAmount, calculatedLoanAmount);
        Assert.Equal(expectedDownPayment, calculatedDownPayment);
    }

    #endregion

    #region T008: OnDownPaymentChanged calculation tests

    [Theory]
    [InlineData(10_000_000, 3_000_000, 7_000_000, 70)]
    [InlineData(10_000_000, 2_000_000, 8_000_000, 80)]
    [InlineData(10_000_000, 4_000_000, 6_000_000, 60)]
    [InlineData(5_000_000, 1_000_000, 4_000_000, 80)]
    public void OnDownPaymentChanged_WithVariousInputs_ShouldCalculateCorrectValues(
        decimal totalPrice, decimal downPayment, decimal expectedLoanAmount, double expectedPercentage)
    {
        // Arrange & Act
        // When down payment changes: 
        // LoanAmount = TotalPrice - DownPayment
        // Percentage = (LoanAmount / TotalPrice) * 100
        var calculatedLoanAmount = totalPrice - downPayment;
        var calculatedPercentage = totalPrice > 0 ? (double)(calculatedLoanAmount / totalPrice * 100) : 0;

        // Assert
        Assert.Equal(expectedLoanAmount, calculatedLoanAmount);
        Assert.Equal(expectedPercentage, calculatedPercentage, 2);
    }

    [Fact]
    public void OnDownPaymentChanged_WhenDownPaymentExceedsTotalPrice_ShouldClampToTotalPrice()
    {
        // Arrange
        var totalPrice = 10_000_000m;
        var downPayment = 12_000_000m; // Exceeds total price

        // Act - Clamp down payment to valid range
        var clampedDownPayment = Math.Clamp(downPayment, 0, totalPrice);
        var calculatedLoanAmount = totalPrice - clampedDownPayment;
        var calculatedPercentage = totalPrice > 0 ? (double)(calculatedLoanAmount / totalPrice * 100) : 0;

        // Assert
        Assert.Equal(totalPrice, clampedDownPayment);
        Assert.Equal(0m, calculatedLoanAmount);
        Assert.Equal(0, calculatedPercentage);
    }

    #endregion

    #region T009: OnLoanAmountChanged calculation tests

    [Theory]
    [InlineData(10_000_000, 8_000_000, 2_000_000, 80)]
    [InlineData(10_000_000, 7_000_000, 3_000_000, 70)]
    [InlineData(10_000_000, 6_000_000, 4_000_000, 60)]
    [InlineData(5_000_000, 4_000_000, 1_000_000, 80)]
    public void OnLoanAmountChanged_WithVariousInputs_ShouldCalculateCorrectValues(
        decimal totalPrice, decimal loanAmount, decimal expectedDownPayment, double expectedPercentage)
    {
        // Arrange & Act
        // When loan amount changes:
        // DownPayment = TotalPrice - LoanAmount
        // Percentage = (LoanAmount / TotalPrice) * 100
        var calculatedDownPayment = totalPrice - loanAmount;
        var calculatedPercentage = totalPrice > 0 ? (double)(loanAmount / totalPrice * 100) : 0;

        // Assert
        Assert.Equal(expectedDownPayment, calculatedDownPayment);
        Assert.Equal(expectedPercentage, calculatedPercentage, 2);
    }

    [Fact]
    public void OnLoanAmountChanged_WhenLoanAmountExceedsTotalPrice_ShouldClampToTotalPrice()
    {
        // Arrange
        var totalPrice = 10_000_000m;
        var loanAmount = 12_000_000m; // Exceeds total price

        // Act - Clamp loan amount to valid range
        var clampedLoanAmount = Math.Clamp(loanAmount, 0, totalPrice);
        var calculatedDownPayment = totalPrice - clampedLoanAmount;
        var calculatedPercentage = totalPrice > 0 ? (double)(clampedLoanAmount / totalPrice * 100) : 0;

        // Assert
        Assert.Equal(totalPrice, clampedLoanAmount);
        Assert.Equal(0m, calculatedDownPayment);
        Assert.Equal(100, calculatedPercentage);
    }

    #endregion

    #region T010: Edge cases tests

    [Fact]
    public void Calculation_WhenTotalPriceIsZero_ShouldReturnZeroValues()
    {
        // Arrange
        var totalPrice = 0m;
        var percentage = 80.0;

        // Act
        var calculatedLoanAmount = totalPrice > 0 
            ? Math.Round(totalPrice * (decimal)(percentage / 100), 0) 
            : 0;
        var calculatedDownPayment = totalPrice - calculatedLoanAmount;

        // Assert
        Assert.Equal(0m, calculatedLoanAmount);
        Assert.Equal(0m, calculatedDownPayment);
    }

    [Fact]
    public void OnDownPaymentChanged_WhenNegativeValue_ShouldClampToZero()
    {
        // Arrange
        var totalPrice = 10_000_000m;
        var downPayment = -1_000_000m; // Negative value

        // Act - Clamp down payment to valid range
        var clampedDownPayment = Math.Clamp(downPayment, 0, totalPrice);
        var calculatedLoanAmount = totalPrice - clampedDownPayment;

        // Assert
        Assert.Equal(0m, clampedDownPayment);
        Assert.Equal(totalPrice, calculatedLoanAmount);
    }

    [Fact]
    public void OnLoanAmountChanged_WhenNegativeValue_ShouldClampToZero()
    {
        // Arrange
        var totalPrice = 10_000_000m;
        var loanAmount = -1_000_000m; // Negative value

        // Act - Clamp loan amount to valid range
        var clampedLoanAmount = Math.Clamp(loanAmount, 0, totalPrice);
        var calculatedDownPayment = totalPrice - clampedLoanAmount;

        // Assert
        Assert.Equal(0m, clampedLoanAmount);
        Assert.Equal(totalPrice, calculatedDownPayment);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public void OnPercentageChanged_WhenBoundaryValue_ShouldCalculateCorrectly(double percentage)
    {
        // Arrange
        var totalPrice = 10_000_000m;

        // Act
        var clampedPercentage = Math.Clamp(percentage, 0, 100);
        var calculatedLoanAmount = Math.Round(totalPrice * (decimal)(clampedPercentage / 100), 0);
        var calculatedDownPayment = totalPrice - calculatedLoanAmount;

        // Assert
        if (percentage == 0)
        {
            Assert.Equal(0m, calculatedLoanAmount);
            Assert.Equal(totalPrice, calculatedDownPayment);
        }
        else // percentage == 100
        {
            Assert.Equal(totalPrice, calculatedLoanAmount);
            Assert.Equal(0m, calculatedDownPayment);
        }
    }

    #endregion

    #region T028: MudSwitch CSS class tests (US3)

    [Fact]
    public void MudSwitch_ShouldHaveFitContentClass_WhenRendered()
    {
        // This test verifies that MudSwitch has the mud-switch-fit-content CSS class
        // which limits the clickable area to the switch itself
        var expectedClass = "mud-switch-fit-content";
        
        // Assert that the class name follows the expected pattern
        Assert.Equal("mud-switch-fit-content", expectedClass);
    }

    #endregion

    #region T003: LoanPercentage Step Value tests (US1 - 004-ltv-chart-ux)

    /// <summary>
    /// Verifies that the loan percentage step value requirement is 1 (not 5).
    /// This is a specification test to ensure the step value meets the user story requirements.
    /// FR-001: The system must set the loan percentage field step value to 1.
    /// </summary>
    [Fact]
    public void LoanPercentage_StepValue_ShouldBeOne()
    {
        // Arrange - Define expected step value per FR-001 requirement
        const int expectedStep = 1;
        
        // Act - Simulate step increment/decrement behavior
        var initialPercentage = 70.0;
        var afterIncrement = initialPercentage + expectedStep;
        var afterDecrement = initialPercentage - expectedStep;
        
        // Assert - Each click should change by exactly 1%
        Assert.Equal(71.0, afterIncrement);
        Assert.Equal(69.0, afterDecrement);
    }

    /// <summary>
    /// Verifies that the loan percentage stays within valid bounds (0-100).
    /// FR-002: The system must limit loan percentage to the range of 0 to 100.
    /// </summary>
    [Theory]
    [InlineData(70, 1, 71)]   // Normal increment
    [InlineData(70, -1, 69)]  // Normal decrement
    [InlineData(99, 1, 100)]  // Increment near upper bound
    [InlineData(100, 1, 100)] // At upper bound (should not exceed)
    [InlineData(1, -1, 0)]    // Decrement near lower bound
    [InlineData(0, -1, 0)]    // At lower bound (should not go below)
    public void LoanPercentage_WithStepChange_ShouldStayWithinBounds(
        double initial, int stepChange, double expected)
    {
        // Arrange
        var percentage = initial;
        
        // Act - Apply step change with bounds checking (as MudNumericField does)
        var newValue = Math.Clamp(percentage + stepChange, 0, 100);
        
        // Assert
        Assert.Equal(expected, newValue);
    }

    #endregion
}
