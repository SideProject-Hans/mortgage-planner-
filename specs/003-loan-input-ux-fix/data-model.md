# Data Model: 房貸輸入介面體驗優化

**Feature**: 003-loan-input-ux-fix  
**Date**: 2025-12-09  
**Status**: Complete

## 概述

本功能涉及兩個既有資料模型的修改，無需建立新模型。

## 模型變更

### LoanInput（既有模型）

**檔案位置**: `src/MortgageCalculator/Models/LoanInput.cs`

**現有屬性**（不變）：

| 屬性名稱 | 型別 | 說明 |
|---------|------|------|
| LoanAmount | decimal | 貸款金額 |
| AnnualInterestRate | double | 年利率 (%) |
| LoanTermYears | int | 貸款年限 |
| RepaymentType | RepaymentType | 還款方式 |
| GracePeriodMonths | int | 寬限期 (月) |
| TotalPrice | decimal | 房屋總價 |
| LoanPercentage | double | 貸款成數 (%) |
| RateStages | List\<RateStage\> | 分段利率設定 |

**新增屬性**（本功能）：

| 屬性名稱 | 型別 | 說明 | 預設值 |
|---------|------|------|--------|
| DownPayment | decimal | 自備款 (元) | 計算值 |

**計算屬性關係**：

```
DownPayment = TotalPrice - LoanAmount
LoanPercentage = (LoanAmount / TotalPrice) × 100
LoanAmount = TotalPrice × (LoanPercentage / 100)
```

**驗證規則**：

- `DownPayment >= 0`
- `DownPayment <= TotalPrice`
- 當 `TotalPrice == 0` 時，`DownPayment = 0`

---

### RateStage（既有模型）

**檔案位置**: `src/MortgageCalculator/Models/RateStage.cs`

**變更項目**：

| 屬性名稱 | 原型別 | 新型別 | 變更原因 |
|---------|--------|--------|----------|
| InterestRate | double | decimal | 避免浮點數精度誤差 |

**變更前**：

```csharp
public class RateStage
{
    public int DurationMonths { get; set; }
    public double InterestRate { get; set; }
}
```

**變更後**：

```csharp
public class RateStage
{
    public int DurationMonths { get; set; }
    public decimal InterestRate { get; set; }
}
```

**影響評估**：

- `MultiStageRateInput.razor`：需更新 MudNumericField 的泛型參數
- `CalculationService`：需檢查利率計算是否受影響（型別轉換）

---

## 資料流程圖

```
┌─────────────────────────────────────────────────────────────────┐
│                    LoanInputForm.razor                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  [房屋總價] ──┬──► OnTotalPriceChanged() ──┐                   │
│               │                             │                   │
│  [貸款成數] ──┼──► OnPercentageChanged() ──┼──► UpdateFields() │
│               │                             │                   │
│  [自備款]   ──┼──► OnDownPaymentChanged() ─┤                   │
│               │                             │                   │
│  [貸款金額] ──┴──► OnLoanAmountChanged() ──┘                   │
│                                                                 │
│                        ↓                                        │
│                   LoanInput                                     │
│            (TotalPrice, LoanAmount,                             │
│             LoanPercentage, DownPayment)                        │
└─────────────────────────────────────────────────────────────────┘
```

## 狀態管理

本功能為純前端計算，無需持久化儲存。所有計算結果存放於 `LoanInput` 物件中，由 Blazor 元件管理狀態。

## 測試資料範例

| 測試案例 | TotalPrice | LoanPercentage | LoanAmount | DownPayment |
|---------|------------|----------------|------------|-------------|
| 基本案例 | 10,000,000 | 80% | 8,000,000 | 2,000,000 |
| 全額貸款 | 10,000,000 | 100% | 10,000,000 | 0 |
| 無貸款 | 10,000,000 | 0% | 0 | 10,000,000 |
| 邊界案例 | 0 | 0% | 0 | 0 |
