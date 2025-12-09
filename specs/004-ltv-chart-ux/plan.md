# Implementation Plan: 貸款成數與圖表 UX 優化

**Branch**: `004-ltv-chart-ux` | **Date**: 2025-12-09 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/004-ltv-chart-ux/spec.md`

## Summary

本功能包含兩項 UX 優化：
1. **貸款成數步進值調整**：將 `MudNumericField` 的 `Step` 屬性從 5 改為 1，讓使用者能以 1% 為單位精確調整貸款成數
2. **還款趨勢圖簡化**：優化 `UpdateChart()` 方法，以 5 年為間隔取樣（短期貸款以 1 年為間隔），並將 X 軸標籤格式改為簡潔的「Y{n}」格式

## Technical Context

**Language/Version**: C# / .NET 10.0  
**Primary Dependencies**: Blazor WebAssembly, MudBlazor 8.15.0  
**Storage**: N/A (無資料模型變更)  
**Testing**: xUnit + bUnit  
**Target Platform**: Web (Blazor WASM)  
**Project Type**: Single project (Blazor WebAssembly)  
**Performance Goals**: UI 更新 < 100ms，圖表渲染流暢  
**Constraints**: 維持現有 MudBlazor 元件 API 使用方式  
**Scale/Scope**: 單一表單元件 + 單一頁面圖表更新

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### I. 程式碼品質標準
- [x] 已定義程式碼風格指南和命名慣例 (VS Code 專案設定)
- [x] 已配置靜態分析工具(linter)和格式化工具 (.NET Analyzers)
- [x] 已建立程式碼審查流程(至少一位審查者)
- [x] SOLID 原則已考慮於架構設計中
- [x] 技術債務追蹤機制已就緒

### II. 測試驅動開發 (非協商性)
- [x] 測試策略已定義(單元/整合/契約/端對端測試範圍)
- [x] 測試覆蓋率目標已設定 (≥ 80% 單元測試覆蓋率)
- [x] 紅-綠-重構 TDD 流程將被遵循
- [x] CI 管線將執行所有測試並阻止失敗合併
- [x] 關鍵業務邏輯已識別並將達到 100% 覆蓋率

### III. 使用者體驗一致性
- [x] UI 元件和設計模式已定義或參考現有設計系統 (MudBlazor)
- [x] 無障礙存取需求已確認 (WCAG 2.1 AA 級 - MudBlazor 內建支援)
- [x] 回應性設計需求已明確(目標裝置和螢幕尺寸)
- [x] 錯誤處理和使用者回饋策略已定義
- [x] 載入狀態和非同步操作回饋機制已計畫

### IV. 效能需求
- [x] 回應時間目標已定義 (UI 更新 < 100ms)
- [x] 效能預算已設定 (圖表資料點 ≤ 10 個)
- [x] 效能測試策略已規劃
- [x] 監控和可觀測性已考慮
- [x] 可擴展性需求已評估

### V. 文件語言需求 (NON-NEGOTIABLE)
- [x] 規格文件將使用繁體中文撰寫 (spec.md, plan.md, research.md 等)
- [x] 使用者面向文件將使用繁體中文 (README, 使用者指南, API 文件)
- [x] 程式碼註解將使用英文 (業務邏輯說明可使用繁體中文)
- [x] 內部溝通將使用繁體中文 (commit 訊息, PR 描述)

**違規理由**: 無違規項目

## Project Structure

### Documentation (this feature)

```text
specs/004-ltv-chart-ux/
├── spec.md              # 功能規格 (已完成)
├── plan.md              # 本文件
├── research.md          # Phase 0 研究成果
├── data-model.md        # Phase 1 資料模型 (N/A - 無資料模型變更)
├── quickstart.md        # Phase 1 快速入門指南
├── contracts/           # Phase 1 介面契約
│   └── interfaces.md    # 介面定義
└── tasks.md             # Phase 2 任務清單 (/speckit.tasks 輸出)
```

### Source Code (repository root)

```text
src/
└── MortgageCalculator/
    ├── Components/
    │   └── LoanInputForm.razor    # 修改: Step="5" → Step="1"
    ├── Pages/
    │   └── CalculatorPage.razor   # 修改: UpdateChart() 方法
    └── Models/
        └── LoanInput.cs           # 無變更

tests/
└── MortgageCalculator.Tests/
    ├── Components/
    │   └── LoanInputFormTests.cs  # 新增: 步進值測試
    └── Pages/
        └── CalculatorPageTests.cs # 新增: 圖表取樣邏輯測試
```

**Structure Decision**: 使用現有專案結構，僅修改既有元件和頁面，無需新增專案或目錄。

## Complexity Tracking

> 無違規項目，無需追蹤
