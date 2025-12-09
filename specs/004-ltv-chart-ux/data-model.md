# Data Model: 貸款成數與圖表 UX 優化

**Feature**: 004-ltv-chart-ux  
**Date**: 2025-12-09  
**Status**: N/A - 無資料模型變更

## 說明

本功能為純 UI 層優化，不涉及任何資料模型變更。

### 不受影響的實體

| 實體 | 說明 |
|------|------|
| `LoanInput` | 貸款輸入參數 (無變更) |
| `LoanResult` | 試算結果 (無變更) |
| `AmortizationScheduleItem` | 攤還明細項目 (無變更) |
| `SavedCalculation` | 儲存的計算記錄 (無變更) |

### 影響範圍

僅影響 UI 元件屬性和顯示邏輯：

1. `MudNumericField.Step` 屬性值
2. `CalculatorPage.UpdateChart()` 方法的取樣邏輯
3. `XAxisLabels` 陣列的格式

無需資料遷移或資料庫變更。
