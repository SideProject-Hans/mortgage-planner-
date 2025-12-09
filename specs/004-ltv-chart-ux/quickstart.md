# Quickstart: 貸款成數與圖表 UX 優化

**Feature**: 004-ltv-chart-ux  
**Date**: 2025-12-09

## 概述

本功能優化房貸試算器的使用者體驗：

1. **貸款成數精細調整**: 步進值從 5% 改為 1%
2. **還款趨勢圖簡化**: 減少資料點數量，使用簡潔標籤

## 快速驗證

### 驗證貸款成數步進值

1. 開啟房貸試算器
2. 開啟「使用房價與成數計算」模式
3. 點擊貸款成數欄位的增加/減少按鈕
4. ✅ 預期: 每次變動 1%

### 驗證還款趨勢圖

1. 設定 30 年期貸款
2. 點擊「試算」按鈕
3. 查看「還款趨勢圖」
4. ✅ 預期: 
   - 資料點數量 ≤ 10 個
   - X 軸標籤格式: Y0, Y5, Y10, ...

### 驗證短期貸款圖表

1. 設定 5 年期貸款
2. 點擊「試算」按鈕
3. 查看「還款趨勢圖」
4. ✅ 預期:
   - 資料點數量 = 6 個 (Y0 ~ Y5)
   - X 軸標籤格式: Y0, Y1, Y2, Y3, Y4, Y5

## 開發指南

### 修改檔案清單

| 檔案 | 變更類型 | 說明 |
|------|---------|------|
| `src/MortgageCalculator/Components/LoanInputForm.razor` | 修改 | Step="5" → Step="1" |
| `src/MortgageCalculator/Pages/CalculatorPage.razor` | 修改 | UpdateChart() 取樣邏輯 |
| `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs` | 新增 | 圖表取樣邏輯測試 |

### 執行測試

```bash
cd tests/MortgageCalculator.Tests
dotnet test
```

### 本地執行

```bash
cd src/MortgageCalculator
dotnet watch run
```

## 驗收標準

- [ ] 貸款成數步進值為 1
- [ ] 30 年期貸款圖表資料點 ≤ 10 個
- [ ] X 軸標籤長度 ≤ 3 字元
- [ ] 短期貸款 (≤5年) 以每年取樣
- [ ] 所有測試通過
