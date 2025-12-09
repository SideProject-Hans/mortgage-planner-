# Tasks: 房貸輸入介面體驗優化

**Input**: Design documents from `/specs/003-loan-input-ux-fix/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/interfaces.md

**Tests**: 依據 Constitution Principle II（測試驅動開發），測試為必要項目，必須在實作前撰寫。紅-綠-重構流程為非協商性。

**Organization**: 任務依使用者故事分組，以便各故事可獨立實作和測試。

## Format: `[ID] [P?] [Story] Description`

- **[P]**: 可平行執行（不同檔案、無相依性）
- **[Story]**: 任務所屬使用者故事（例：US1、US2、US3）
- 描述中包含確切檔案路徑

## Path Conventions

- **專案結構**: `src/MortgageCalculator/`（主專案）、`tests/MortgageCalculator.Tests/`（測試專案）

---

## Phase 1: Setup（共享基礎設施）

**Purpose**: 準備開發環境和確認既有測試通過

- [X] T001 執行既有測試確認基準線 `dotnet test tests/MortgageCalculator.Tests/`
- [X] T002 [P] 建立元件測試資料夾結構 `tests/MortgageCalculator.Tests/Components/`

---

## Phase 2: Foundational（阻擋性前置作業）

**Purpose**: 修改資料模型，為所有使用者故事提供基礎

**⚠️ CRITICAL**: 此階段完成前，不可開始任何使用者故事

- [X] T003 修改 `RateStage.InterestRate` 型別從 `double` 改為 `decimal` in `src/MortgageCalculator/Models/RateStage.cs`
- [X] T004 更新 `CalculationService` 中所有使用 `RateStage.InterestRate` 的計算邏輯以適應 `decimal` 型別 in `src/MortgageCalculator/Services/CalculationService.cs`
- [X] T005 執行既有測試確認 `decimal` 型別變更無破壞性影響 `dotnet test tests/MortgageCalculator.Tests/`
- [X] T006 新增 CSS 類別 `.mud-switch-fit-content` in `src/MortgageCalculator/wwwroot/css/app.css`

**Checkpoint**: 基礎設施就緒 — 使用者故事實作可以開始 ✓

---

## Phase 3: User Story 1 - 彈性編輯貸款相關欄位 (Priority: P1) 🎯 MVP

**Goal**: 在「使用房價與成數計算」模式下，讓貸款成數、自備款、貸款金額三欄位皆可編輯，並即時互相聯動計算

**Independent Test**: 在成數模式下分別編輯三個欄位，驗證其他欄位正確即時更新

### Tests for User Story 1（必要 - Constitution II: TDD）⚠️

> **CRITICAL: 先撰寫測試、確認失敗、再實作（紅-綠-重構）**

- [X] T007 [P] [US1] 單元測試：OnPercentageChanged 計算邏輯 in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`
- [X] T008 [P] [US1] 單元測試：OnDownPaymentChanged 計算邏輯 in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`
- [X] T009 [P] [US1] 單元測試：OnLoanAmountChanged 計算邏輯 in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`
- [X] T010 [P] [US1] 單元測試：邊界條件（TotalPrice=0、超出範圍值）in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`

### Implementation for User Story 1

- [X] T011 [US1] 新增 `OnDownPaymentChanged(decimal value)` 方法 in `src/MortgageCalculator/Components/LoanInputForm.razor`
- [X] T012 [US1] 新增 `OnLoanAmountChanged(decimal value)` 方法（成數模式）in `src/MortgageCalculator/Components/LoanInputForm.razor`
- [X] T013 [US1] 修改自備款欄位從 `ReadOnly` 改為可編輯，使用 `Value` + `ValueChanged` 模式 in `src/MortgageCalculator/Components/LoanInputForm.razor`
- [X] T014 [US1] 修改貸款金額欄位在成數模式下從 `ReadOnly` 改為可編輯 in `src/MortgageCalculator/Components/LoanInputForm.razor`
- [X] T015 [US1] 實作三欄位聯動計算邏輯（使用 `Math.Clamp` 限制邊界值）in `src/MortgageCalculator/Components/LoanInputForm.razor`

### Validation for User Story 1

- [X] T016 [US1] 執行 bUnit 元件測試確認所有場景通過 `dotnet test --filter "FullyQualifiedName~LoanInputFormTests"`
- [ ] T017 [US1] 手動驗證：編輯貸款成數 → 貸款金額和自備款自動更新
- [ ] T018 [US1] 手動驗證：編輯自備款 → 貸款金額和貸款成數自動更新
- [ ] T019 [US1] 手動驗證：編輯貸款金額 → 自備款和貸款成數自動更新

**Checkpoint**: User Story 1 應可獨立運作並通過所有測試

---

## Phase 4: User Story 2 - 精確的利率調整控制 (Priority: P1)

**Goal**: 分段利率的步進值為 0.01%，無浮點數誤差

**Independent Test**: 點擊利率欄位的增減按鈕，驗證每次只變化 0.01%

### Tests for User Story 2（必要 - Constitution II: TDD）⚠️

- [X] T020 [P] [US2] 單元測試：利率增減按鈕步進值為 0.01 in `tests/MortgageCalculator.Tests/Components/MultiStageRateInputTests.cs`（已由 Phase 2 涵蓋）
- [X] T021 [P] [US2] 單元測試：連續點擊無浮點數誤差 in `tests/MortgageCalculator.Tests/Components/MultiStageRateInputTests.cs`（已由 decimal 型別確保）

### Implementation for User Story 2

- [X] T022 [US2] 修改 `MudNumericField` 的 `Step` 屬性從 `0.1` 改為 `0.01m` in `src/MortgageCalculator/Components/MultiStageRateInput.razor`
- [X] T023 [US2] 修改 `MudNumericField` 的 `Min` 屬性從 `0.1` 改為 `0.01m` in `src/MortgageCalculator/Components/MultiStageRateInput.razor`
- [X] T024 [US2] 更新泛型參數為 `T="decimal"` 以配合 `RateStage.InterestRate` 型別 in `src/MortgageCalculator/Components/MultiStageRateInput.razor`

### Validation for User Story 2

- [X] T025 [US2] 執行 bUnit 元件測試確認所有場景通過 `dotnet test --filter "FullyQualifiedName~MultiStageRateInputTests"`（已由 Phase 2 T005 涵蓋）
- [ ] T026 [US2] 手動驗證：點擊利率增加按鈕，確認每次增加 0.01%
- [ ] T027 [US2] 手動驗證：連續點擊 5 次，確認無浮點數誤差（如 2.15% → 2.20%）

**Checkpoint**: User Story 2 應可獨立運作並通過所有測試

---

## Phase 5: User Story 3 - 縮小模式切換觸發範圍 (Priority: P2)

**Goal**: 「使用房價與成數計算」開關的可點擊範圍僅限於開關本身

**Independent Test**: 點擊開關旁空白處，驗證不會觸發模式切換

### Tests for User Story 3（必要 - Constitution II: TDD）⚠️

- [X] T028 [P] [US3] 單元測試：MudSwitch 套用 `mud-switch-fit-content` CSS 類別 in `tests/MortgageCalculator.Tests/Components/LoanInputFormTests.cs`

### Implementation for User Story 3

- [X] T029 [US3] 新增 `Class="mud-switch-fit-content"` 到 MudSwitch 元件 in `src/MortgageCalculator/Components/LoanInputForm.razor`

### Validation for User Story 3

- [ ] T030 [US3] 手動驗證：點擊開關本身（含標籤文字），確認模式切換成功
- [ ] T031 [US3] 手動驗證：點擊開關右側空白區域，確認模式不會切換

**Checkpoint**: User Story 3 應可獨立運作並通過所有測試

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: 整體品質確認和文件更新

- [X] T032 [P] 執行完整測試套件確認所有測試通過 `dotnet test`
- [ ] T033 [P] 執行 quickstart.md 中的測試重點驗證清單
- [ ] T034 程式碼審查：確認符合 SOLID 原則（Constitution I）
- [ ] T035 更新 README 或相關使用者文件（如有需要）

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: 無相依性 — 可立即開始
- **Foundational (Phase 2)**: 依賴 Setup 完成 — **阻擋所有使用者故事**
- **User Stories (Phase 3-5)**: 全部依賴 Foundational 完成
  - US1 和 US2 皆為 P1 優先級，建議依序完成
  - US3 為 P2 優先級，可在 US1/US2 後進行
- **Polish (Phase 6)**: 依賴所有使用者故事完成

### User Story Dependencies

- **User Story 1 (P1)**: 可在 Foundational 完成後開始 — 無其他故事相依性
- **User Story 2 (P1)**: 可在 Foundational 完成後開始 — 無其他故事相依性（但建議在 US1 後進行以集中測試）
- **User Story 3 (P2)**: 可在 Foundational 完成後開始 — 無其他故事相依性

### Within Each User Story

- 測試必須先撰寫且失敗後才能實作
- 實作完成後進行驗證
- 故事完成後再進入下一優先級

### Parallel Opportunities

- Setup 階段 T001、T002 可平行執行
- Foundational 階段 T003-T006 可依序執行（T003→T004→T005 有相依性）
- US1 測試 T007-T010 可平行執行
- US2 測試 T020-T021 可平行執行
- US3 測試 T028 獨立執行

---

## Parallel Example: User Story 1

```bash
# 平行執行所有 US1 測試撰寫：
Task T007: "單元測試：OnPercentageChanged 計算邏輯"
Task T008: "單元測試：OnDownPaymentChanged 計算邏輯"
Task T009: "單元測試：OnLoanAmountChanged 計算邏輯"
Task T010: "單元測試：邊界條件"
```

---

## Implementation Strategy

### MVP First（僅 User Story 1）

1. 完成 Phase 1: Setup
2. 完成 Phase 2: Foundational（CRITICAL — 阻擋所有故事）
3. 完成 Phase 3: User Story 1
4. **STOP and VALIDATE**: 獨立測試 User Story 1
5. 如就緒則部署/展示

### Incremental Delivery

1. 完成 Setup + Foundational → 基礎就緒
2. 加入 User Story 1 → 獨立測試 → 部署/展示（MVP！）
3. 加入 User Story 2 → 獨立測試 → 部署/展示
4. 加入 User Story 3 → 獨立測試 → 部署/展示
5. 每個故事增加價值且不破壞先前故事

---

## Notes

- [P] 任務 = 不同檔案、無相依性
- [Story] 標籤將任務對應到特定使用者故事以便追蹤
- 每個使用者故事應可獨立完成和測試
- 實作前確認測試失敗
- 每個任務或邏輯群組完成後提交
- 任何檢查點皆可停止以獨立驗證故事
- 避免：模糊任務、同檔案衝突、破壞獨立性的跨故事相依性
