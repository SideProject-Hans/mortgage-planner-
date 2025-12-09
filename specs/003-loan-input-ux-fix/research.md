# Research: 房貸輸入介面體驗優化

**Feature**: 003-loan-input-ux-fix  
**Date**: 2025-12-09  
**Status**: Complete

## 研究概述

本研究針對三項技術需求進行調查：
1. MudBlazor MudNumericField 的雙向綁定與即時計算模式
2. 浮點數精度問題與 decimal/double 的最佳實踐
3. MudSwitch 觸發範圍的 CSS 控制方式

---

## 研究項目 1：三欄位即時同步計算

### 問題描述
在「使用房價與成數計算」模式下，需要讓三個欄位（貸款成數、自備款、貸款金額）皆可編輯，並在任一欄位變更時即時更新其他兩個欄位。

### 研究發現

**MudNumericField 綁定模式**：
- `@bind-Value`：雙向綁定，適用於獨立欄位
- `Value` + `ValueChanged`：單向綁定 + 手動處理，適用於需要攔截變更的場景

**推薦方案**：使用 `Value` + `ValueChanged` 模式
```razor
<MudNumericField 
    Value="LoanInput.LoanPercentage" 
    ValueChanged="@((double v) => OnPercentageChanged(v))" />
```

**計算公式**：
- 當貸款成數變更：`LoanAmount = TotalPrice × (Percentage / 100)`，`DownPayment = TotalPrice - LoanAmount`
- 當自備款變更：`LoanAmount = TotalPrice - DownPayment`，`Percentage = (LoanAmount / TotalPrice) × 100`
- 當貸款金額變更：`DownPayment = TotalPrice - LoanAmount`，`Percentage = (LoanAmount / TotalPrice) × 100`

### 決策
- **Decision**: 使用 `Value` + `ValueChanged` 模式處理三欄位聯動
- **Rationale**: 現有程式碼已採用此模式處理房屋總價和貸款成數，擴展至三欄位具一致性
- **Alternatives considered**: `@bind-Value` + `@bind-Value:after` — 需要 .NET 7+ 語法，但無法阻止無效值傳播

---

## 研究項目 2：利率精度問題

### 問題描述
分段利率的 `Step` 屬性目前設為 0.1，但使用者報告看到 `0.0000000000000001%` 這類浮點數誤差。

### 研究發現

**根本原因**：
- `double` 型別的二進位浮點數表示法無法精確表示 0.01 等十進位小數
- MudNumericField 的增減按鈕直接對 `double` 值加減 `Step`，累積誤差

**解決方案選項**：

| 方案 | 優點 | 缺點 |
|------|------|------|
| 使用 `decimal` 型別 | 十進位精度、無累積誤差 | 需修改 `RateStage` 模型 |
| 保持 `double` + `Math.Round` | 最小變更 | 每次計算都需四捨五入 |
| 自訂 ValueChanged 處理 | 精確控制 | 程式碼較繁瑣 |

**MudBlazor 支援**：
- `MudNumericField<T>` 支援 `decimal` 型別
- `Step` 屬性接受 `decimal` 值

### 決策
- **Decision**: 將 `RateStage.InterestRate` 改為 `decimal` 型別，Step 設為 `0.01m`
- **Rationale**: 利率計算對精度敏感，`decimal` 為金融計算的標準選擇
- **Alternatives considered**: 保持 `double` + 四捨五入 — 無法根本解決問題，且每次顯示都需處理

---

## 研究項目 3：MudSwitch 觸發範圍

### 問題描述
「使用房價與成數計算」開關的可點擊區域延伸到整行空白處，導致使用者在附近點擊時意外觸發切換。

### 研究發現

**MudSwitch 渲染結構**：
```html
<label class="mud-switch">
  <input type="checkbox">
  <span class="mud-switch-span">...</span>
  <span class="mud-switch-label">使用房價與成數計算</span>
</label>
```

**問題根源**：
- MudSwitch 被放在 `<MudItem xs="12">` 內，佔據整行
- `<label>` 元素預設為 `display: inline-flex`，但父容器 `xs="12"` 使其擴展

**解決方案選項**：

| 方案 | 優點 | 缺點 |
|------|------|------|
| 限制 MudItem 寬度 | 簡單 | 可能影響響應式佈局 |
| CSS 限制 label 寬度 | 精確控制 | 需新增 CSS |
| 包裝在 div 內 | 隔離影響 | 增加 DOM 層級 |

### 決策
- **Decision**: 使用 CSS `width: fit-content` 限制 MudSwitch 的寬度
- **Rationale**: 最小變更，不影響現有佈局結構
- **Alternatives considered**: 限制 MudItem 寬度 — 可能在小螢幕上造成意外換行

**實作方式**：
```css
.mud-switch-percentage-mode {
    width: fit-content;
}
```

```razor
<MudSwitch Class="mud-switch-percentage-mode" ... />
```

---

## 邊界條件處理

### 房屋總價為 0 或未輸入
- **Decision**: 當 `TotalPrice <= 0` 時，將貸款成數、自備款、貸款金額皆設為 0，並顯示提示訊息
- **Rationale**: 避免除以零錯誤，並給使用者明確指引

### 輸入值超出範圍
- **Decision**: 使用 `Math.Clamp` 限制輸入值在有效範圍內
  - 貸款成數：0% ~ 100%
  - 自備款：0 ~ TotalPrice
  - 貸款金額：0 ~ TotalPrice
- **Rationale**: 提供即時修正而非僅顯示錯誤訊息

### 利率最小值
- **Decision**: 將 `Min` 從 `0.1` 改為 `0.01`，以支援更低的利率設定
- **Rationale**: 部分政策性貸款利率低於 0.1%

---

## 結論

所有研究項目均已完成，無待釐清事項。技術方案如下：

| 需求 | 技術方案 |
|------|----------|
| 三欄位即時同步 | `Value` + `ValueChanged` 模式，三個 handler 方法 |
| 利率精度 | `RateStage.InterestRate` 改為 `decimal`，Step=0.01m |
| 開關觸發範圍 | CSS `width: fit-content` 限制 |
