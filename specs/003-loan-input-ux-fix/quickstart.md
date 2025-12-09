# Quickstart: 房貸輸入介面體驗優化

**Feature**: 003-loan-input-ux-fix  
**Date**: 2025-12-09

## 快速瞭解本功能

本功能改善房貸計算器的輸入介面體驗，包含三項修正：

1. **三欄位即時同步**：在「使用房價與成數計算」模式下，貸款成數、自備款、貸款金額皆可編輯
2. **利率精度修正**：分段利率的步進值從 0.1% 改為 0.01%
3. **開關觸發範圍**：縮小模式切換開關的可點擊區域

## 開發環境設定

### 前置需求

- .NET 10.0 SDK
- Visual Studio 2022 / VS Code + C# Dev Kit
- 瀏覽器（支援 WebAssembly）

### 執行專案

```bash
cd src/MortgageCalculator
dotnet watch run
```

瀏覽器開啟 `https://localhost:5001`

### 執行測試

```bash
cd tests/MortgageCalculator.Tests
dotnet test
```

## 關鍵檔案

| 檔案 | 用途 |
|------|------|
| `Components/LoanInputForm.razor` | 主要修改：三欄位同步邏輯 |
| `Components/MultiStageRateInput.razor` | 修改：利率 Step 精度 |
| `Models/RateStage.cs` | 修改：InterestRate 型別改為 decimal |
| `wwwroot/css/app.css` | 新增：開關樣式 |

## 實作要點

### 1. 三欄位同步計算

```csharp
// 當自備款變更時
private void OnDownPaymentChanged(decimal value)
{
    var downPayment = Math.Clamp(value, 0, LoanInput.TotalPrice);
    LoanInput.LoanAmount = LoanInput.TotalPrice - downPayment;
    if (LoanInput.TotalPrice > 0)
    {
        LoanInput.LoanPercentage = (double)(LoanInput.LoanAmount / LoanInput.TotalPrice * 100);
    }
}
```

### 2. 利率精度

```razor
<MudNumericField 
    T="decimal"
    @bind-Value="stage.InterestRate" 
    Step="0.01m" 
    Min="0.01m" />
```

### 3. 開關範圍限制

```css
.mud-switch-fit-content {
    width: fit-content !important;
}
```

## 測試重點

- [ ] 編輯貸款成數 → 驗證貸款金額和自備款自動更新
- [ ] 編輯自備款 → 驗證貸款金額和貸款成數自動更新
- [ ] 編輯貸款金額 → 驗證自備款和貸款成數自動更新
- [ ] 點擊利率增加按鈕 → 驗證每次增加 0.01%
- [ ] 點擊開關旁空白處 → 驗證不會觸發切換

## 相關文件

- [規格文件](./spec.md)
- [實作計畫](./plan.md)
- [研究筆記](./research.md)
- [資料模型](./data-model.md)
- [介面契約](./contracts/interfaces.md)
