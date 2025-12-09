---
description: "Task list for LTV and Chart UX optimization feature"
---

# Tasks: 貸款成數與圖表 UX 優化

**Input**: Design documents from `/specs/004-ltv-chart-ux/`
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, data-model.md ✅, quickstart.md ✅, contracts/ ✅

**Tests**: 依據 Constitution Principle II (Test-Driven Development)，測試是必須的，且必須在實作前撰寫。紅-綠-重構流程是不可協商的。

**Organization**: 任務依使用者故事分組，以便獨立實作和測試每個故事。

## Format: `[ID] [P?] [Story] Description`

- **[P]**: 可平行執行（不同檔案，無相依性）
- **[Story]**: 此任務屬於哪個使用者故事（例如 US1, US2）
- 描述中包含確切的檔案路徑

## Path Conventions

- **專案結構**: `src/MortgageCalculator/` 為主專案, `tests/MortgageCalculator.Tests/` 為測試專案

---

## Phase 1: Setup (環境準備)

**Purpose**: 確認開發環境與測試框架就緒

- [ ] T001 確認專案結構並驗證測試框架可正常執行 (`dotnet test`)
- [ ] T002 [P] 建立測試檔案 `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs`

---

## Phase 2: Foundational (基礎設施)

**Purpose**: 無需基礎設施變更 - 本功能為純 UI 優化

**⚠️ 說明**: 本功能不涉及資料模型、服務介面或外部整合的變更，所有修改僅在 UI 元件層。

**Checkpoint**: 可直接進入使用者故事實作

---

## Phase 3: User Story 1 - 精細調整貸款成數 (Priority: P1) 🎯 MVP

**Goal**: 讓使用者能以 1% 為單位精確調整貸款成數

**Independent Test**: 使用者在貸款成數欄位點擊增加或減少按鈕，每次變動 1%

### Tests for User Story 1 (MANDATORY - Constitution II: TDD) ⚠️

> **CRITICAL: 先撰寫測試，確認測試失敗，再實作 (Red-Green-Refactor)**

- [ ] T003 [P] [US1] 單元測試：驗證 LoanPercentage 欄位 Step 屬性為 1 in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`

### Implementation for User Story 1

- [ ] T004 [US1] 修改 `src/MortgageCalculator/Components/LoanInputForm.razor` 第 17 行: `Step="5"` → `Step="1"`

### Validation for User Story 1

- [ ] T005 [US1] 執行測試並驗證通過 (`dotnet test --filter "LoanPercentage"`)
- [ ] T006 [US1] 手動驗證：開啟應用程式，確認貸款成數步進值為 1

**Checkpoint**: 此時 User Story 1 應完全可用且通過測試

---

## Phase 4: User Story 2 - 簡化還款趨勢圖顯示 (Priority: P1)

**Goal**: 優化還款趨勢圖，以 5 年間隔取樣（短期貸款 1 年間隔），使用簡潔的 "Y{n}" 標籤格式

**Independent Test**: 使用者進行 30 年期貸款試算後，圖表顯示不超過 10 個資料點，標籤格式為 Y0, Y5, Y10...

### Tests for User Story 2 (MANDATORY - Constitution II: TDD) ⚠️

> **CRITICAL: 先撰寫測試，確認測試失敗，再實作 (Red-Green-Refactor)**

- [ ] T007 [P] [US2] 單元測試：驗證長期貸款 (30年) 圖表取樣間隔為 5 年 in `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs`
- [ ] T008 [P] [US2] 單元測試：驗證短期貸款 (5年) 圖表取樣間隔為 1 年 in `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs`
- [ ] T009 [P] [US2] 單元測試：驗證 X 軸標籤格式為 "Y{n}" in `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs`
- [ ] T010 [P] [US2] 單元測試：驗證圖表資料點數量不超過 10 個 in `tests/MortgageCalculator.Tests/Pages/CalculatorPageTests.cs`

### Implementation for User Story 2

- [ ] T011 [US2] 提取 `UpdateChart()` 取樣邏輯為可測試的靜態方法 in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [ ] T012 [US2] 實作新的取樣邏輯：長期貸款 (>5年) 5 年間隔, 短期貸款 (≤5年) 1 年間隔 in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [ ] T013 [US2] 修改 X 軸標籤格式為 "Y{n}" in `src/MortgageCalculator/Pages/CalculatorPage.razor`

### Validation for User Story 2

- [ ] T014 [US2] 執行測試並驗證通過 (`dotnet test --filter "Chart"`)
- [ ] T015 [US2] 手動驗證：30 年期貸款圖表資料點數量 ≤ 10
- [ ] T016 [US2] 手動驗證：5 年期貸款圖表以每年取樣
- [ ] T017 [US2] 手動驗證：X 軸標籤格式為 Y0, Y5, Y10...

**Checkpoint**: 此時 User Story 1 和 2 都應獨立運作且通過測試

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: 最終驗證與清理

- [ ] T018 執行 `quickstart.md` 中的所有驗證步驟
- [ ] T019 執行完整測試套件 (`dotnet test`)
- [ ] T020 程式碼審查：確認符合 SOLID 原則 (Constitution I)
- [ ] T021 更新 `specs/004-ltv-chart-ux/checklists/requirements.md` 確認所有需求已實現

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: 無相依性 - 可立即開始
- **Foundational (Phase 2)**: N/A - 本功能無基礎設施需求
- **User Story 1 (Phase 3)**: 依賴 Setup 完成
- **User Story 2 (Phase 4)**: 依賴 Setup 完成，可與 US1 平行進行
- **Polish (Phase 5)**: 依賴所有使用者故事完成

### User Story Dependencies

- **User Story 1 (P1)**: 獨立於其他故事 - 僅修改 LoanInputForm.razor
- **User Story 2 (P1)**: 獨立於其他故事 - 僅修改 CalculatorPage.razor

### Within Each User Story

- 測試必須先撰寫並失敗，然後再實作
- 實作後立即執行測試驗證
- 每個故事完成後進行手動驗證

### Parallel Opportunities

- T002 可與 T001 平行
- T003, T007, T008, T009, T010 可平行撰寫（不同測試方法）
- User Story 1 和 User Story 2 可平行進行（不同檔案，無相依性）

---

## Parallel Example: All Test Tasks

```bash
# 所有測試任務可平行進行（不同測試類別/方法）:
Task: T003 [P] [US1] 單元測試：驗證 Step 屬性
Task: T007 [P] [US2] 單元測試：驗證長期貸款取樣間隔
Task: T008 [P] [US2] 單元測試：驗證短期貸款取樣間隔
Task: T009 [P] [US2] 單元測試：驗證 X 軸標籤格式
Task: T010 [P] [US2] 單元測試：驗證資料點數量上限
```

---

## Parallel Example: User Stories

```bash
# User Story 1 和 User Story 2 可完全平行（不同檔案）:

# 開發者 A - User Story 1:
Task: T003 → T004 → T005, T006

# 開發者 B - User Story 2:
Task: T007, T008, T009, T010 → T011 → T012 → T013 → T014, T015, T016, T017
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. 完成 Phase 1: Setup
2. 完成 Phase 3: User Story 1 (貸款成數步進值)
3. **停止並驗證**: 獨立測試 User Story 1
4. 可部署/展示 MVP

### Incremental Delivery

1. 完成 Setup → 環境就緒
2. 完成 User Story 1 → 獨立測試 → 部署 (MVP!)
3. 完成 User Story 2 → 獨立測試 → 部署
4. 完成 Polish → 最終驗證 → 合併到主分支

### Parallel Team Strategy

若有多位開發者：

1. 團隊一起完成 Setup
2. Setup 完成後：
   - 開發者 A: User Story 1 (LoanInputForm.razor)
   - 開發者 B: User Story 2 (CalculatorPage.razor)
3. 故事獨立完成並整合

---

## Notes

- 本功能為純 UI 優化，無資料模型或服務變更
- 所有修改都是向後相容的
- [P] 標記表示可平行執行（不同檔案，無相依性）
- [US1], [US2] 標記對應 spec.md 中的使用者故事
- 每個使用者故事可獨立完成和測試
- 在實作前驗證測試失敗
- 每個任務或邏輯群組完成後提交
- 在任何 checkpoint 停止以獨立驗證故事
