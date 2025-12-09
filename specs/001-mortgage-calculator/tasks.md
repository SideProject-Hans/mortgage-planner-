# Tasks: 房屋貸款試算工具

**Feature Branch**: `001-mortgage-calculator`
**Status**: Pending
**Spec**: [spec.md](./spec.md)
**Plan**: [plan.md](./plan.md)

## Phase 1: Setup (Project Initialization)

- [x] T001 Create Blazor WebAssembly project `MortgageCalculator` in `src/MortgageCalculator`
- [x] T002 Create xUnit Test project `MortgageCalculator.Tests` in `tests/MortgageCalculator.Tests`
- [x] T003 Add project reference from Tests to Main project
- [x] T004 Install NuGet package `MudBlazor` to `src/MortgageCalculator`
- [x] T005 Install NuGet package `Blazored.LocalStorage` to `src/MortgageCalculator`
- [x] T006 Install NuGet package `bUnit` to `tests/MortgageCalculator.Tests`
- [x] T007 Configure MudBlazor services and theme in `src/MortgageCalculator/Program.cs`
- [x] T008 Configure Blazored.LocalStorage services in `src/MortgageCalculator/Program.cs`
- [x] T009 Add MudBlazor imports to `src/MortgageCalculator/_Imports.razor`
- [x] T010 Setup `MainLayout.razor` with `MudLayout`, `MudAppBar`, and `MudMainContent` in `src/MortgageCalculator/Layout/MainLayout.razor`
- [x] T011 Create basic folder structure (`Components`, `Models`, `Services`) in `src/MortgageCalculator`

## Phase 2: Foundational (Core Models & Interfaces)

- [x] T012 [P] Create `RepaymentType` enum in `src/MortgageCalculator/Models/Enums.cs`
- [x] T013 [P] Create `LoanInput` class with validation attributes in `src/MortgageCalculator/Models/LoanInput.cs`
- [x] T014 [P] Create `AmortizationScheduleItem` class in `src/MortgageCalculator/Models/AmortizationScheduleItem.cs`
- [x] T015 [P] Create `LoanResult` class in `src/MortgageCalculator/Models/LoanResult.cs`
- [x] T016 [P] Define `ICalculationService` interface in `src/MortgageCalculator/Services/ICalculationService.cs`
- [x] T017 [P] Define `IStorageService` interface in `src/MortgageCalculator/Services/IStorageService.cs`
- [x] T018 Create `CalculationService` stub implementation in `src/MortgageCalculator/Services/CalculationService.cs`
- [x] T019 Register `ICalculationService` in `src/MortgageCalculator/Program.cs`

## Phase 3: User Story 1 - Basic Calculation (Core Logic)

**Goal**: Implement the core mortgage calculation logic and a basic UI to verify it.
**Independent Test**: Input 800k, 30 years, 2.5%, EqualPrincipalAndInterest -> Verify monthly payment ~31,603.

- [x] T020 [US1] Create unit test `Calculate_EqualPrincipalAndInterest_ReturnsCorrectValues` in `tests/MortgageCalculator.Tests/Services/CalculationServiceTests.cs`
- [x] T021 [US1] Implement `Calculate` method for `EqualPrincipalAndInterest` in `src/MortgageCalculator/Services/CalculationService.cs`
- [x] T022 [US1] Create unit test `Calculate_EqualPrincipal_ReturnsCorrectValues` in `tests/MortgageCalculator.Tests/Services/CalculationServiceTests.cs`
- [x] T023 [US1] Implement `Calculate` method for `EqualPrincipal` in `src/MortgageCalculator/Services/CalculationService.cs`
- [x] T024 [US1] Create `CalculatorPage.razor` with basic `MudNumericField` inputs in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T025 [US1] Implement `Calculate` button logic to call service and display simple text result in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T026 [US1] Add basic validation display (DataAnnotationsValidator) in `src/MortgageCalculator/Pages/CalculatorPage.razor`

## Phase 4: Input Enhancements (US2, US3, US4)

**Goal**: Add Amount/Percentage toggle, Multi-stage rates, and Default values.
**Independent Test**: Toggle to Percentage, enter Total Price & Percentage -> Verify Amount calculated.

- [x] T027 [US2] Add `TotalPrice` and `LoanPercentage` properties to `LoanInput` in `src/MortgageCalculator/Models/LoanInput.cs`
- [x] T028 [US2] Add Toggle Switch for Input Mode (Amount vs Percentage) in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T029 [US2] Implement logic to auto-calculate Amount from Percentage (and vice-versa) in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T030 [US3] Create `RateStage` class in `src/MortgageCalculator/Models/RateStage.cs`
- [x] T031 [US3] Add `RateStages` list to `LoanInput` in `src/MortgageCalculator/Models/LoanInput.cs`
- [x] T032 [US3] Create `MultiStageRateInput.razor` component for adding/removing stages in `src/MortgageCalculator/Components/MultiStageRateInput.razor`
- [x] T033 [US3] Update `CalculationService` to handle multi-stage rate logic in `src/MortgageCalculator/Services/CalculationService.cs`
- [x] T034 [US3] Add unit tests for multi-stage calculation in `tests/MortgageCalculator.Tests/Services/CalculationServiceTests.cs`
- [x] T035 [US4] Initialize `LoanInput` with default values (30 years, 2.5%) in `src/MortgageCalculator/Pages/CalculatorPage.razor`

## Phase 5: Result Visualization (US5, US6, US7)

**Goal**: Display Charts, Summary Cards, and Amortization Table.
**Independent Test**: Verify Chart renders and Table shows correct number of rows (e.g., 360 for 30 years).

- [x] T036 [US5] Add Summary Cards (Monthly Payment, Total Interest, Total Payment) using `MudCard` in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T037 [US5] Add `MudChart` (Line Chart) to display Principal vs Interest over time in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T038 [US5] Implement logic to map `LoanResult.Schedule` to Chart Series in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T039 [US6] Create `AmortizationTable.razor` component in `src/MortgageCalculator/Components/AmortizationTable.razor`
- [x] T040 [US6] Implement `<Virtualize>` for efficient rendering of 360+ rows in `src/MortgageCalculator/Components/AmortizationTable.razor`
- [x] T041 [US6] Integrate `AmortizationTable` into `CalculatorPage.razor`
- [x] T042 [US7] Apply `N0` format string to all currency displays in `src/MortgageCalculator/Pages/CalculatorPage.razor` and components

## Phase 6: Comparison Features (US8, US9, US10, US11)

**Goal**: Enable A/B comparison with side-by-side view and diff analysis.
**Independent Test**: Add Scenario B -> Verify two columns of results and diffs are shown.

- [x] T043 [US8] Refactor `CalculatorPage` to manage `LoanInput` for Scenario A and Scenario B in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T044 [US8] Create `ComparisonView.razor` to display side-by-side results in `src/MortgageCalculator/Components/ComparisonView.razor`
- [x] T045 [US9] Implement logic to calculate differences (Diff = B - A) in `src/MortgageCalculator/Components/ComparisonView.razor`
- [x] T046 [US9] Add visual cues (Green/Red colors) for differences in `src/MortgageCalculator/Components/ComparisonView.razor`
- [x] T047 [US10] Update `MudChart` to support displaying 2 scenarios (4 lines total or toggleable) in `src/MortgageCalculator/Pages/CalculatorPage.razor`
- [x] T048 [US11] Implement "Copy A to B" and "Swap" buttons in `src/MortgageCalculator/Pages/CalculatorPage.razor`

## Phase 7: Storage & History (US13)

**Goal**: Save and Load calculations using LocalStorage.
**Independent Test**: Save a calculation, refresh page, Load it -> Verify inputs are restored.

- [x] T049 [US13] Create `SavedCalculation` model in `src/MortgageCalculator/Models/SavedCalculation.cs`
- [x] T050 [US13] Implement `StorageService` using `Blazored.LocalStorage` in `src/MortgageCalculator/Services/StorageService.cs`
- [x] T051 [US13] Create `SavedCalculationsList.razor` component (Drawer or Dialog) in `src/MortgageCalculator/Components/SavedCalculationsList.razor`
- [x] T052 [US13] Add "Save" button and "History" button to `CalculatorPage.razor`
- [x] T053 [US13] Implement Load logic to populate `LoanInput` from saved record in `src/MortgageCalculator/Pages/CalculatorPage.razor`

## Phase 8: Affordability & Down Payment (US14, US15)

**Goal**: Add Affordability Checker and Down Payment info.
**Independent Test**: Enter Income -> Verify suggested loan amount.

- [x] T054 [US14] Create `AffordabilityChecker.razor` component in `src/MortgageCalculator/Components/AffordabilityChecker.razor` (Implemented as `AffordabilityPage.razor`)
- [x] T055 [US14] Implement affordability logic (Income * 30% rule) in `src/MortgageCalculator/Components/AffordabilityChecker.razor` (Implemented in `CalculationService.cs`)
- [x] T056 [US15] Add Down Payment display section (Total - Loan) in `src/MortgageCalculator/Components/LoanInputForm.razor`
- [x] T057 [US15] Add logic to update Down Payment when Total Price or Loan Amount changes in `src/MortgageCalculator/Components/LoanInputForm.razor`

## Phase 9: Polish & Final Review

- [x] T058 [P] Review and refine UI spacing, typography, and colors (MudBlazor Theme) in `src/MortgageCalculator/Layout/MainLayout.razor`
- [x] T059 [P] Ensure all error messages are in Traditional Chinese in `src/MortgageCalculator/Resources/Strings.resx` (if using resx) or hardcoded
- [x] T060 [P] Verify responsive layout on mobile view (Chrome DevTools)
- [x] T061 [P] Perform manual end-to-end test of all User Scenarios

## Dependencies

- Phase 1 & 2 must be completed before Phase 3.
- Phase 3 is the prerequisite for all subsequent phases.
- Phase 4, 5, 6, 7, 8 can be developed somewhat in parallel, but Phase 4 (Inputs) strongly influences Phase 5 & 6.
- Recommended Order: 1 -> 2 -> 3 -> 4 -> 5 -> 6 -> 7 -> 8 -> 9

## Implementation Strategy

1.  **MVP (Phase 1-3)**: Get the basic calculator working with correct math.
2.  **Enhanced UX (Phase 4-5)**: Make it usable with better inputs and charts.
3.  **Advanced Features (Phase 6-8)**: Add the "Planner" capabilities (Compare, Save, Affordability).
4.  **Polish (Phase 9)**: Finalize for release.

---

## Issue Log

### ISS-001: MudBlazor Component Rendering Errors (2025-12-09)

**狀態**: ✅ 已解決

**錯誤訊息**:
- `Missing <MudPopoverProvider />, please add it to your layout`
- `Could not find 'mudElementRef.getBoundingClientRect' ('mudElementRef' was undefined)`
- `Could not find 'mudElementRef.addOnBlurEvent' ('mudElementRef' was undefined)`

**根本原因**:
MudBlazor 初始設定不完整，導致元件無法正確渲染：
1. `MainLayout.razor` 缺少 `<MudPopoverProvider />` - 這是 MudBlazor 彈出式元件（Popover、Menu、Select 等）的必要 Provider
2. `index.html` 缺少 MudBlazor 的 CSS 和 JavaScript 引用 - 導致 JS Interop 函式 (`mudElementRef`) 未定義

**修復方式**:

| 檔案 | 變更內容 |
|------|----------|
| `src/MortgageCalculator/Layout/MainLayout.razor` | 加入 `<MudPopoverProvider />` |
| `src/MortgageCalculator/wwwroot/index.html` | 加入 MudBlazor CSS (`_content/MudBlazor/MudBlazor.min.css`) |
| `src/MortgageCalculator/wwwroot/index.html` | 加入 MudBlazor JS (`_content/MudBlazor/MudBlazor.min.js`) |
| `src/MortgageCalculator/wwwroot/index.html` | 加入 Roboto 字型引用 |
| `src/MortgageCalculator/wwwroot/index.html` | 移除未使用的 Bootstrap CSS |

**教訓學習**:
- MudBlazor 需要完整的設定才能正常運作，包含：
  1. **NuGet 套件** (`MudBlazor`)
  2. **服務註冊** (`builder.Services.AddMudServices()`)
  3. **所有 Providers** (`MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider`, `MudSnackbarProvider`)
  4. **CSS 引用** (`_content/MudBlazor/MudBlazor.min.css`)
  5. **JS 引用** (`_content/MudBlazor/MudBlazor.min.js`)
- 官方文件：https://mudblazor.com/getting-started/installation

**相關任務**: T010 (Setup MainLayout.razor)
