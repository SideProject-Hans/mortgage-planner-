# Feature Specification: 房屋貸款試算工具

**Feature Branch**: `001-mortgage-calculator`  
**Created**: 2025-12-05  
**Status**: Draft  
**Input**: User description: "房屋貸款試算小工具 - 輸入房屋總價、貸款金額或成數（可切換）、貸款年限（預設30年）、利率（預設2.5%）、還款方式（等額本息或等額本金），試算後顯示每月還款金額"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - 基本房貸試算 (Priority: P1)

身為一位購屋者，我想要輸入貸款條件並立即看到每月還款金額，以便評估我的財務負擔能力。

**Why this priority**: 這是工具的核心價值，沒有試算功能就沒有產品存在的意義。使用者最基本的需求就是知道每月要還多少錢。

**Independent Test**: 可透過輸入貸款金額、年限、利率後點擊試算，驗證是否正確顯示每月還款金額。

**Acceptance Scenarios**:

1. **Given** 使用者在試算頁面，**When** 輸入貸款金額 800 萬、年限 30 年、利率 2.5%、選擇等額本息，**Then** 系統顯示每月還款金額約 31,603 元
2. **Given** 使用者在試算頁面，**When** 輸入貸款金額 800 萬、年限 30 年、利率 2.5%、選擇等額本金，**Then** 系統顯示首月還款金額約 38,889 元，並顯示每月遞減說明
3. **Given** 使用者未輸入任何資料，**When** 點擊試算按鈕，**Then** 系統提示必填欄位需要填寫

---

### User Story 2 - 貸款金額輸入切換 (Priority: P1)

身為一位購屋者，我想要能夠在「直接輸入貸款金額」和「輸入貸款成數」之間切換，因為有時我知道確切金額，有時我只知道想貸幾成。

**Why this priority**: 這是使用者明確要求的功能，且直接影響輸入體驗。不同使用者有不同的思考方式，此功能讓工具更具彈性。

**Independent Test**: 可透過切換輸入模式，驗證系統能正確接受金額或成數輸入，並正確計算結果。

**Acceptance Scenarios**:

1. **Given** 使用者選擇「貸款金額」模式，**When** 輸入 800 萬，**Then** 系統以 800 萬作為貸款金額進行試算
2. **Given** 使用者選擇「貸款成數」模式且已輸入房屋總價 1000 萬，**When** 輸入 8 成，**Then** 系統自動計算貸款金額為 800 萬並進行試算
3. **Given** 使用者在「貸款成數」模式，**When** 未輸入房屋總價就輸入成數，**Then** 系統提示需先輸入房屋總價

---

### User Story 3 - 預設值便利輸入 (Priority: P2)

身為一位購屋者，我希望常用的參數已有預設值，這樣我只需調整少數欄位就能快速試算。

**Why this priority**: 預設值能大幅提升使用效率，讓使用者更快得到結果。但這是增強體驗，不是核心功能。

**Independent Test**: 可透過開啟試算頁面，驗證年限預設為 30 年、利率預設為 2.5%。

**Acceptance Scenarios**:

1. **Given** 使用者首次開啟試算頁面，**When** 頁面載入完成，**Then** 貸款年限顯示預設值 30 年
2. **Given** 使用者首次開啟試算頁面，**When** 頁面載入完成，**Then** 貸款利率顯示預設值 2.5%
3. **Given** 使用者修改預設值，**When** 進行試算，**Then** 系統使用修改後的數值計算

---

### Edge Cases

- 當使用者輸入負數或零作為貸款金額時，系統應顯示錯誤提示
- 當使用者輸入超過 100% 的貸款成數時，系統應顯示錯誤提示
- 當使用者輸入利率為 0% 時，系統應能正確計算（無利息情況）
- 當使用者輸入非常大的金額（例如超過 10 億）時，系統應能正確處理或提示超出範圍
- 當使用者輸入小數點年限時（例如 20.5 年），系統應如何處理
- 當使用者快速切換貸款金額/成數模式時，已輸入的數值應如何處理

## Requirements *(mandatory)*

### Functional Requirements

**輸入相關**
- **FR-001**: 系統必須提供房屋總價輸入欄位
- **FR-002**: 系統必須提供貸款金額輸入欄位
- **FR-003**: 系統必須提供貸款成數輸入欄位
- **FR-004**: 系統必須提供「貸款金額」與「貸款成數」之間的切換機制
- **FR-005**: 系統必須提供貸款年限輸入欄位，預設值為 30 年
- **FR-006**: 系統必須提供貸款利率輸入欄位，預設值為 2.5%
- **FR-007**: 系統必須提供還款方式選擇，包含「等額本息」與「等額本金」兩種選項

**計算相關**
- **FR-008**: 當選擇「等額本息」時，系統必須使用標準等額本息公式計算每月還款金額
- **FR-009**: 當選擇「等額本金」時，系統必須計算首月還款金額及每月遞減金額
- **FR-010**: 當使用「貸款成數」模式時，系統必須根據房屋總價自動計算實際貸款金額

**輸出相關**
- **FR-011**: 系統必須顯示計算後的每月還款金額
- **FR-012**: 當選擇「等額本金」時，系統必須顯示每月還款遞減的說明資訊

**驗證相關**
- **FR-013**: 系統必須驗證所有必填欄位已填寫後才能進行試算
- **FR-014**: 系統必須驗證輸入數值為有效的正數
- **FR-015**: 系統必須驗證貸款成數在 1% 至 100% 之間

### Key Entities

- **貸款條件（Loan Parameters）**: 包含房屋總價、貸款金額、貸款成數、年限、利率、還款方式等計算所需的輸入參數
- **試算結果（Calculation Result）**: 包含每月還款金額、總利息、總還款金額等計算輸出

### Assumptions

- 利率為年利率，系統內部轉換為月利率進行計算
- 貸款年限以「年」為單位，系統內部轉換為月數進行計算
- 金額單位為新台幣（元），不需處理其他幣別
- 等額本息公式：M = P × [r(1+r)^n] / [(1+r)^n - 1]，其中 M=月付款、P=本金、r=月利率、n=總期數
- 等額本金公式：首月還款 = (P/n) + P×r，之後每月遞減 (P/n)×r

### Constitutional Requirements *(mandatory)*

#### Code Quality (Constitution I)

- [x] Code review process defined
- [x] Static analysis tools configured
- [x] Naming conventions and style guide followed
- [x] SOLID principles considered in design

#### Testing (Constitution II - NON-NEGOTIABLE)

- [x] TDD approach will be followed (red-green-refactor)
- [x] Unit test coverage target: ≥ 80% (critical paths: 100%)
- [x] Integration tests defined for component interactions
- [ ] Contract tests defined for API boundaries（N/A - 純前端應用，無 API 邊界）
- [x] All tests will be written BEFORE implementation

#### User Experience (Constitution III)

- [x] UI components follow design system or have consistency plan
- [x] Accessibility requirements defined (WCAG 2.1 AA)
- [x] Responsive design requirements specified
- [x] Error messages are user-friendly and actionable
- [x] Loading states and async feedback mechanisms planned

#### Performance (Constitution IV)

- [x] Response time targets defined (計算結果應在 100ms 內顯示)
- [x] Performance budget set (bundle size < 200KB gzipped)
- [x] Performance testing strategy planned
- [ ] Monitoring and observability considered（N/A - 初期版本不需要）
- [x] Scalability requirements assessed（純前端計算，無伺服器負載考量）

#### Documentation Language (Constitution V - NON-NEGOTIABLE)

- [x] This specification written in Traditional Chinese
- [x] User-facing documentation will be in Traditional Chinese
- [x] Code comments will be in English (business logic may use Traditional Chinese)
- [x] Internal communication will use Traditional Chinese

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 使用者可在 30 秒內完成一次完整的房貸試算
- **SC-002**: 95% 的使用者首次使用即能成功完成試算，無需額外指引
- **SC-003**: 計算結果與銀行官方試算工具的誤差在 1 元以內
- **SC-004**: 頁面在行動裝置上完全可用，所有功能皆可操作
- **SC-005**: 所有輸入欄位皆有明確的標籤與說明，符合無障礙標準
