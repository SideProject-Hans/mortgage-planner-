# Interfaces: 房貸輸入介面體驗優化

**Feature**: 003-loan-input-ux-fix  
**Date**: 2025-12-09  
**Status**: Complete

## 概述

本功能主要涉及 Blazor Razor 元件的介面調整，無新增服務介面。

## 元件介面定義

### LoanInputForm.razor

**Parameters（公開屬性）**：

```csharp
[Parameter] public LoanInput LoanInput { get; set; }
[Parameter] public EventCallback OnValidSubmit { get; set; }
```

**內部狀態**：

```csharp
private bool isPercentageMode = false;
private bool isMultiStageRate = false;
```

**新增方法（本功能）**：

| 方法名稱 | 簽章 | 說明 |
|---------|------|------|
| OnDownPaymentChanged | `void OnDownPaymentChanged(decimal value)` | 自備款變更處理 |
| OnLoanAmountChanged | `void OnLoanAmountChanged(decimal value)` | 貸款金額變更處理（成數模式） |
| RecalculateFromPercentage | `void RecalculateFromPercentage()` | 從貸款成數重算其他欄位 |
| RecalculateFromDownPayment | `void RecalculateFromDownPayment()` | 從自備款重算其他欄位 |
| RecalculateFromLoanAmount | `void RecalculateFromLoanAmount()` | 從貸款金額重算其他欄位 |

---

### MultiStageRateInput.razor

**Parameters（公開屬性）**：

```csharp
[Parameter] public List<RateStage> RateStages { get; set; }
```

**變更項目**：

- `InterestRate` 欄位的 `Step` 屬性從 `0.1` 改為 `0.01m`
- `Min` 屬性從 `0.1` 改為 `0.01m`

---

## UI 元件契約

### MudNumericField 使用規範

**貸款成數欄位**：

```razor
<MudNumericField 
    T="double"
    Value="LoanInput.LoanPercentage" 
    ValueChanged="@((double v) => OnPercentageChanged(v))"
    Label="貸款成數 (%)"
    Min="0" 
    Max="100" 
    Step="5" />
```

**自備款欄位**（新增可編輯）：

```razor
<MudNumericField 
    T="decimal"
    Value="@DownPayment" 
    ValueChanged="@((decimal v) => OnDownPaymentChanged(v))"
    Label="自備款 (元)"
    Min="0" 
    Max="@LoanInput.TotalPrice" />
```

**貸款金額欄位**（成數模式下可編輯）：

```razor
<MudNumericField 
    T="decimal"
    Value="LoanInput.LoanAmount" 
    ValueChanged="@((decimal v) => OnLoanAmountChanged(v))"
    Label="貸款金額 (元)"
    Min="0" 
    Max="@LoanInput.TotalPrice" />
```

**分段利率欄位**：

```razor
<MudNumericField 
    T="decimal"
    @bind-Value="stage.InterestRate"
    Label="利率 (%)"
    Min="0.01m" 
    Step="0.01m" />
```

---

## CSS 介面

### 新增 CSS 類別

**檔案**: `wwwroot/css/app.css`

```css
/* 限制 MudSwitch 的可點擊範圍 */
.mud-switch-fit-content {
    width: fit-content !important;
}
```

**使用方式**：

```razor
<MudSwitch 
    Class="mud-switch-fit-content"
    @bind-Value="isPercentageMode" 
    Label="使用房價與成數計算" 
    Color="Color.Primary" />
```

---

## 事件流程契約

### 欄位變更事件序列

```text
User Input → ValueChanged Event → Handler Method → Recalculate → StateHasChanged
```

### 計算優先順序

1. 使用者編輯的欄位值優先
2. 根據公式計算其他欄位
3. 套用邊界值限制（Clamp）
4. 更新 UI 顯示

### 錯誤處理

- 除以零：當 `TotalPrice == 0` 時，所有計算欄位設為 0
- 溢位：使用 `Math.Clamp` 限制在有效範圍內
- 精度：`decimal` 型別確保利率計算精度
