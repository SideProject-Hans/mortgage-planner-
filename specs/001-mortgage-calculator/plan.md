# Implementation Plan: 房屋貸款試算工具

**Branch**: `001-mortgage-calculator` | **Date**: 2025-12-09 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-mortgage-calculator/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

本功能為一個純前端的房屋貸款試算工具，使用 C# Blazor WebAssembly (.NET 8) 開發。主要功能包含：
1.  支援「貸款金額」與「貸款成數」雙向輸入與計算。
2.  支援「等額本息」與「等額本金」兩種還款方式。
3.  支援「固定利率」與「多階段浮動利率」設定。
4.  提供即時試算結果，包含每月還款金額、總利息、還款曲線圖（使用 MudBlazor 圖表元件）。
5.  提供完整的還款明細表。
6.  支援 A/B 方案比較功能。
7.  提供房貸負擔能力檢查與自備款計算。
8.  使用 LocalStorage 儲存試算記錄。
9.  採用單頁式儀表板設計 (Single Page Dashboard)。

## Technical Context

<!--
  ACTION REQUIRED: Replace the content in this section with the technical details
  for the project. The structure here is presented in advisory capacity to guide
  the iteration process.
-->

**Language/Version**: C# (.NET 8)
**Primary Dependencies**: Blazor WebAssembly, MudBlazor, Blazored.LocalStorage
**Storage**: LocalStorage (via Blazored.LocalStorage)
**Testing**: xUnit, bUnit (for Blazor components)
**Target Platform**: WebAssembly (Static Web App)
**Project Type**: Single Page Application (SPA)
**Performance Goals**: UI response < 100ms, Chart rendering < 500ms, Bundle size optimized
**Constraints**: Pure frontend, Offline capable
**Scale/Scope**: Single user, local data only

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. 程式碼品質標準
- [x] 已定義程式碼風格指南和命名慣例 (遵循 C# 標準)
- [x] 已配置靜態分析工具(linter)和格式化工具 (dotnet format)
- [x] 已建立程式碼審查流程(至少一位審查者)
- [x] SOLID 原則已考慮於架構設計中
- [x] 技術債務追蹤機制已就緒

### II. 測試驅動開發 (非協商性)
- [x] 測試策略已定義(單元/整合/契約/端對端測試範圍) (主要依賴 bUnit 進行元件測試)
- [x] 測試覆蓋率目標已設定 (≥ 80% 單元測試覆蓋率)
- [x] 紅-綠-重構 TDD 流程將被遵循
- [x] CI 管線將執行所有測試並阻止失敗合併
- [x] 關鍵業務邏輯已識別並將達到 100% 覆蓋率 (貸款計算核心邏輯)

### III. 使用者體驗一致性
- [x] UI 元件和設計模式已定義或參考現有設計系統 (MudBlazor)
- [x] 無障礙存取需求已確認 (WCAG 2.1 AA 級)
- [x] 回應性設計需求已明確(目標裝置和螢幕尺寸) (MudBlazor Grid System)
- [x] 錯誤處理和使用者回饋策略已定義
- [x] 載入狀態和非同步操作回饋機制已計畫

### IV. 效能需求
- [x] 回應時間目標已定義 (API < 200ms, UI FCP < 1.5s)
- [x] 效能預算已設定 (套件大小、圖片、記憶體)
- [x] 效能測試策略已規劃(負載測試、效能回歸測試)
- [x] 監控和可觀測性已考慮(指標、日誌、追蹤)
- [x] 可擴展性需求已評估(預期使用者量、資料量)

### V. 文件語言需求 (NON-NEGOTIABLE)
- [x] 規格文件將使用繁體中文撰寫 (spec.md, plan.md, research.md 等)
- [x] 使用者面向文件將使用繁體中文 (README, 使用者指南, API 文件)
- [x] 程式碼註解將使用英文 (業務邏輯說明可使用繁體中文)
- [x] 內部溝通將使用繁體中文 (commit 訊息, PR 描述)

**違規理由** (僅在有未通過檢查點時填寫):
| 違規項目 | 為何需要 | 被拒絕的更簡單替代方案及理由 |
|---------|---------|---------------------------|
| N/A | | |

## Project Structure

### Documentation (this feature)

```text
specs/001-mortgage-calculator/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)
<!--
  ACTION REQUIRED: Replace the placeholder tree below with the concrete layout
  for this feature. Delete unused options and expand the chosen structure with
  real paths (e.g., apps/admin, packages/something). The delivered plan must
  not include Option labels.
-->

```text
src/
├── MortgageCalculator/          # Blazor WebAssembly Project
│   ├── Components/              # Reusable UI Components
│   ├── Layout/                  # Main Layouts
│   ├── Pages/                   # Page Components (Dashboard)
│   ├── Models/                  # Data Models
│   ├── Services/                # Calculation & Storage Services
│   ├── wwwroot/                 # Static Assets
│   ├── App.razor
│   ├── Program.cs
│   └── _Imports.razor

tests/
├── MortgageCalculator.Tests/    # xUnit Test Project
│   ├── Components/              # bUnit Component Tests
│   ├── Services/                # Unit Tests for Services
│   └── Helpers/                 # Test Helpers
```

**Structure Decision**: 採用標準的 Blazor WebAssembly 專案結構，將核心邏輯與 UI 元件分離。測試專案獨立，使用 bUnit 進行元件測試。

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | | |
