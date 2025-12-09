# Implementation Plan: 房貸輸入介面體驗優化

**Branch**: `003-loan-input-ux-fix` | **Date**: 2025-12-09 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/003-loan-input-ux-fix/spec.md`

## Summary

本計畫實作三項 UI/UX 改善：

1. **三欄位即時同步計算**：在「使用房價與成數計算」模式下，貸款成數、自備款、貸款金額三欄位皆可編輯，且即時互相聯動計算
2. **利率精度修正**：將分段利率的 `Step` 屬性從 0.1 改為 0.01，並使用 `Math.Round` 避免浮點數誤差
3. **縮小開關觸發範圍**：透過 CSS 限制 MudSwitch 的可點擊區域，避免空白處誤觸

技術方案基於現有 Blazor WebAssembly + MudBlazor 8.x 架構，僅修改元件邏輯和樣式，無需新增套件。

## Technical Context

**Language/Version**: C# 13 / .NET 10.0  
**Primary Dependencies**: Blazor WebAssembly, MudBlazor 8.15.0, Blazored.LocalStorage 4.5.0  
**Storage**: N/A（本功能不涉及儲存層）  
**Testing**: xUnit 2.9.3 + bUnit 2.2.2 + coverlet  
**Target Platform**: WebAssembly (瀏覽器)  
**Project Type**: Single Blazor WASM project  
**Performance Goals**: 欄位同步計算 < 100ms、無 UI 卡頓  
**Constraints**: 純前端計算，無 API 呼叫；利率顯示精度為小數點後兩位  
**Scale/Scope**: 單一頁面元件修改，影響 2 個 Razor 元件、1 個 Model 類別

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. 程式碼品質標準

- [x] 已定義程式碼風格指南和命名慣例（遵循 .NET 編碼規範）
- [x] 已配置靜態分析工具(linter)和格式化工具（專案啟用 Nullable、ImplicitUsings）
- [x] 已建立程式碼審查流程(至少一位審查者)（GitHub PR review）
- [x] SOLID 原則已考慮於架構設計中（單一職責：計算邏輯封裝於方法中）
- [x] 技術債務追蹤機制已就緒（GitHub Issues）

### II. 測試驅動開發 (非協商性)

- [x] 測試策略已定義（單元測試：計算邏輯；元件測試：bUnit 驗證 UI 行為）
- [x] 測試覆蓋率目標已設定 (≥ 80% 單元測試覆蓋率)
- [x] 紅-綠-重構 TDD 流程將被遵循
- [x] CI 管線將執行所有測試並阻止失敗合併
- [x] 關鍵業務邏輯已識別並將達到 100% 覆蓋率（三欄位同步計算邏輯）

### III. 使用者體驗一致性

- [x] UI 元件和設計模式已定義或參考現有設計系統（MudBlazor Material Design）
- [x] 無障礙存取需求已確認 (WCAG 2.1 AA 級)（MudBlazor 內建支援）
- [x] 回應性設計需求已明確（使用 MudGrid xs/sm 響應斷點）
- [x] 錯誤處理和使用者回饋策略已定義（ValidationMessage 元件）
- [x] 載入狀態和非同步操作回饋機制已計畫（N/A：純同步計算）

### IV. 效能需求

- [x] 回應時間目標已定義 (計算同步 < 100ms)
- [x] 效能預算已設定 (無額外 JS/CSS bundle)
- [x] 效能測試策略已規劃（手動驗證即時計算無延遲）
- [x] 監控和可觀測性已考慮（瀏覽器 DevTools）
- [x] 可擴展性需求已評估（N/A：純前端計算）

### V. 文件語言需求 (NON-NEGOTIABLE)

- [x] 規格文件將使用繁體中文撰寫 (spec.md, plan.md, research.md 等)
- [x] 使用者面向文件將使用繁體中文 (README, 使用者指南, API 文件)
- [x] 程式碼註解將使用英文 (業務邏輯說明可使用繁體中文)
- [x] 內部溝通將使用繁體中文 (commit 訊息, PR 描述)

**違規理由**: 無違規項目

## Project Structure

### Documentation (this feature)

```text
specs/003-loan-input-ux-fix/
├── spec.md              # 功能規格（已完成）
├── plan.md              # 本文件（實作計畫）
├── research.md          # Phase 0 研究輸出
├── data-model.md        # Phase 1 資料模型設計
├── quickstart.md        # Phase 1 快速開始指南
├── contracts/           # Phase 1 介面契約
│   └── interfaces.md    # 元件介面定義
├── checklists/
│   └── requirements.md  # 需求檢查清單（已完成）
└── tasks.md             # Phase 2 任務清單（/speckit.tasks 建立）
```

### Source Code (repository root)

```text
src/MortgageCalculator/
├── Components/
│   ├── LoanInputForm.razor      # 主要修改：三欄位同步計算、開關觸發範圍
│   └── MultiStageRateInput.razor # 修改：利率 Step 精度
├── Models/
│   └── LoanInput.cs             # 可能新增：DownPayment 屬性
└── wwwroot/css/
    └── app.css                  # 新增：開關樣式覆寫

tests/MortgageCalculator.Tests/
├── Components/
│   └── LoanInputFormTests.cs    # 新增：bUnit 元件測試
└── Services/
    └── CalculationServiceTests.cs # 現有測試檔案
```

**Structure Decision**: 採用現有單一專案結構，僅修改 `Components/` 下的 2 個 Razor 元件，並在 `tests/` 下新增 bUnit 元件測試。

## Complexity Tracking

無需追蹤 — Constitution Check 全數通過，無違規項目。

---

## Post-Design Constitution Re-Check

*Re-evaluated after Phase 1 design completion (2025-12-09)*

| 類別 | 狀態 | 備註 |
|------|------|------|
| I. 程式碼品質 | ✅ 通過 | 設計遵循 SOLID，計算邏輯封裝於獨立方法 |
| II. TDD | ✅ 通過 | 測試策略已定義於 data-model.md 測試資料範例 |
| III. UX 一致性 | ✅ 通過 | 使用 MudBlazor 現有元件，無新增 UI 模式 |
| IV. 效能需求 | ✅ 通過 | 純同步計算，無額外網路請求 |
| V. 文件語言 | ✅ 通過 | 所有文件皆為繁體中文 |

**結論**: Phase 1 設計完成，所有 Constitution 檢查項目通過。可進入 Phase 2 任務分解。

---

## Phase 1 Artifacts Summary

| 文件 | 狀態 | 說明 |
|------|------|------|
| research.md | ✅ 完成 | 技術研究：MudNumericField 綁定、decimal 精度、CSS 範圍限制 |
| data-model.md | ✅ 完成 | LoanInput 新增 DownPayment、RateStage.InterestRate 改為 decimal |
| contracts/interfaces.md | ✅ 完成 | 元件介面定義、新增方法簽章、CSS 類別規範 |
| quickstart.md | ✅ 完成 | 開發環境設定、關鍵檔案、實作要點 |

