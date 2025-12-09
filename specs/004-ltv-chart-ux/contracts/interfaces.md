# Interfaces: 貸款成數與圖表 UX 優化

**Feature**: 004-ltv-chart-ux  
**Date**: 2025-12-09

## Component Contracts

### LoanInputForm.razor

**修改項目**: `MudNumericField` 的 `Step` 屬性

| 屬性 | 修改前 | 修改後 |
|------|--------|--------|
| Step | 5 | 1 |

**行為契約**:

- 當使用者點擊增加按鈕時，`LoanPercentage` 增加 1
- 當使用者點擊減少按鈕時，`LoanPercentage` 減少 1
- `LoanPercentage` 值範圍: 0 ~ 100 (由 `Min` 和 `Max` 屬性控制)

### CalculatorPage.razor - UpdateChart()

**輸入**: `LoanResult result` (包含 `Schedule` 還款明細)

**輸出**:

- `string[] XAxisLabels`: X 軸標籤陣列
- `List<ChartSeries> Series`: 圖表資料系列

**行為契約**:

```text
WHEN result.Schedule.Count / 12 > 5 (長期貸款)
THEN 取樣間隔 = 5 年
AND XAxisLabels = ["Y0", "Y5", "Y10", ...] (最多 10 個標籤)

WHEN result.Schedule.Count / 12 <= 5 (短期貸款)
THEN 取樣間隔 = 1 年
AND XAxisLabels = ["Y0", "Y1", "Y2", ...] (最多 6 個標籤)
```

**取樣邏輯**:

```csharp
// Pseudo-code contract
int totalYears = (int)Math.Ceiling(result.Schedule.Count / 12.0);
int intervalYears = totalYears <= 5 ? 1 : 5;

for (int year = 0; year <= totalYears; year += intervalYears)
{
    // 取得該年份的資料點
    // 年份 0 = 初始本金
    // 年份 N = 第 N*12 期的剩餘本金
}
```

## Test Contracts

### LoanInputForm 測試契約

```csharp
// 測試: 步進值為 1
[Fact]
public void LoanPercentageField_StepValue_ShouldBeOne()
{
    // Given: LoanInputForm 元件
    // When: 檢查 LoanPercentage 欄位的 Step 屬性
    // Then: Step = 1
}
```

### CalculatorPage 測試契約

```csharp
// 測試: 長期貸款取樣間隔
[Theory]
[InlineData(30, 7)]  // 30年 → Y0, Y5, Y10, Y15, Y20, Y25, Y30 = 7 個點
[InlineData(20, 5)]  // 20年 → Y0, Y5, Y10, Y15, Y20 = 5 個點
[InlineData(10, 3)]  // 10年 → Y0, Y5, Y10 = 3 個點
public void UpdateChart_LongTermLoan_ShouldSampleEvery5Years(int years, int expectedDataPoints)

// 測試: 短期貸款取樣間隔
[Theory]
[InlineData(5, 6)]   // 5年 → Y0, Y1, Y2, Y3, Y4, Y5 = 6 個點
[InlineData(3, 4)]   // 3年 → Y0, Y1, Y2, Y3 = 4 個點
public void UpdateChart_ShortTermLoan_ShouldSampleEveryYear(int years, int expectedDataPoints)

// 測試: X 軸標籤格式
[Fact]
public void UpdateChart_XAxisLabels_ShouldUseYearFormat()
{
    // Given: 試算結果
    // When: 更新圖表
    // Then: XAxisLabels 格式為 "Y{n}"
}
```

## Integration Points

本功能為 UI 層修改，無外部整合點。

所有修改僅影響:

1. `LoanInputForm.razor` - 步進值屬性
2. `CalculatorPage.razor` - 圖表更新邏輯

不影響:

- `ICalculationService` 介面
- `LoanInput` / `LoanResult` 資料模型
- `IStorageService` 儲存服務
