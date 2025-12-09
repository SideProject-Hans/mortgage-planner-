# Feature Specification: 房貸輸入介面體驗優化

**Feature Branch**: `003-loan-input-ux-fix`  
**Created**: 2025-12-09  
**Status**: Draft  
**Input**: 使用者描述: "調整房貸計算器輸入介面：1.在使用房價與成數計算模式下，貸款成數、自備款、貸款金額應可編輯並即時計算 2.縮小切換模式觸發範圍 3.修正分段利率調整精度為0.01%"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - 彈性編輯貸款相關欄位 (Priority: P1)

作為房貸計算使用者，當我啟用「使用房價與成數計算」模式時，我希望能夠編輯貸款成數、自備款或貸款金額中的任一欄位，系統會自動計算並同步更新其他相關欄位，讓我能更彈性地進行房貸規劃。

**Why this priority**: 這是使用者回饋的核心需求，目前自備款和貸款金額無法編輯，限制了使用者的操作彈性。此功能直接影響產品的實用性和使用者滿意度。

**Independent Test**: 可透過在成數模式下分別編輯貸款成數、自備款、貸款金額三個欄位，驗證其他欄位是否正確即時更新。

**Acceptance Scenarios**:

1. **Given** 使用者啟用「使用房價與成數計算」模式且房屋總價為 1000 萬元, **When** 使用者將貸款成數改為 80%, **Then** 貸款金額自動更新為 800 萬元，自備款自動更新為 200 萬元
2. **Given** 使用者啟用「使用房價與成數計算」模式且房屋總價為 1000 萬元, **When** 使用者將自備款改為 300 萬元, **Then** 貸款金額自動更新為 700 萬元，貸款成數自動更新為 70%
3. **Given** 使用者啟用「使用房價與成數計算」模式且房屋總價為 1000 萬元, **When** 使用者將貸款金額改為 600 萬元, **Then** 自備款自動更新為 400 萬元，貸款成數自動更新為 60%
4. **Given** 使用者編輯自備款使其超過房屋總價, **When** 輸入值大於房屋總價, **Then** 系統將自備款限制為房屋總價，貸款金額設為 0，貸款成數設為 0%

---

### User Story 2 - 精確的利率調整控制 (Priority: P1)

作為房貸計算使用者，當我在分段利率設定中調整利率時，我希望每次點擊增減按鈕時利率只變化 0.01%，而不是產生極小的浮點數誤差，確保利率數值精確且易於使用。

**Why this priority**: 這是一個明顯的 bug，會導致使用者看到如 `0.0000000000000001%` 這樣不合理的數值，嚴重影響使用體驗和數據可信度。

**Independent Test**: 可透過在分段利率設定中點擊利率欄位的增減按鈕，驗證利率每次只變化 0.01%。

**Acceptance Scenarios**:

1. **Given** 使用者在分段利率設定中，利率目前為 2.00%, **When** 使用者點擊增加按鈕一次, **Then** 利率變為 2.01%
2. **Given** 使用者在分段利率設定中，利率目前為 2.00%, **When** 使用者點擊減少按鈕一次, **Then** 利率變為 1.99%
3. **Given** 使用者在分段利率設定中，利率目前為 2.15%, **When** 使用者連續點擊增加按鈕 5 次, **Then** 利率變為 2.20%（無浮點數誤差）

---

### User Story 3 - 縮小模式切換觸發範圍 (Priority: P2)

作為房貸計算使用者，我希望「使用房價與成數計算」的切換開關有明確的可點擊範圍，避免在開關附近的空白區域誤觸而意外切換模式。

**Why this priority**: 這是使用體驗的改善項目，雖然不影響核心功能，但會減少使用者的操作挫折感。

**Independent Test**: 可透過點擊開關本身和其周圍空白區域，驗證只有點擊開關本身才會觸發模式切換。

**Acceptance Scenarios**:

1. **Given** 使用者在貸款輸入表單頁面, **When** 使用者點擊「使用房價與成數計算」開關本身（包含文字標籤）, **Then** 模式切換成功
2. **Given** 使用者在貸款輸入表單頁面, **When** 使用者點擊開關右側的空白區域, **Then** 模式不會切換

---

### Edge Cases

- 當房屋總價為 0 或未輸入時，貸款成數、自備款、貸款金額的編輯行為如何處理？（假設：顯示 0 或空值，但不阻止輸入）
- 當使用者輸入的貸款金額大於房屋總價時，系統如何處理？（假設：將貸款金額限制為房屋總價）
- 當使用者輸入負數時，系統如何處理？（假設：輸入欄位已設定最小值為 0，不允許負數）
- 利率的最小值和最大值限制？（假設：最小 0.01%，最大 100%）

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: 系統 MUST 允許使用者在「使用房價與成數計算」模式下編輯貸款成數欄位，並即時計算更新貸款金額和自備款
- **FR-002**: 系統 MUST 允許使用者在「使用房價與成數計算」模式下編輯自備款欄位，並即時計算更新貸款金額和貸款成數
- **FR-003**: 系統 MUST 允許使用者在「使用房價與成數計算」模式下編輯貸款金額欄位，並即時計算更新自備款和貸款成數
- **FR-004**: 系統 MUST 確保三個欄位（貸款成數、自備款、貸款金額）的數值始終保持數學一致性（貸款金額 + 自備款 = 房屋總價，貸款成數 = 貸款金額 / 房屋總價 × 100）
- **FR-005**: 系統 MUST 將分段利率欄位的調整步進值設為 0.01（代表 0.01%）
- **FR-006**: 系統 MUST 確保利率數值在顯示和計算時不會出現浮點數精度誤差（如顯示為 2.01% 而非 2.0099999999999998%）
- **FR-007**: 系統 MUST 將「使用房價與成數計算」開關的可點擊範圍限制在開關元件本身（包含標籤文字），不擴展到整行空白區域
- **FR-008**: 系統 MUST 在編輯任一欄位時提供即時視覺回饋，讓使用者知道其他欄位正在同步更新
- **FR-009**: 系統 MUST 防止輸入無效值（如負數、超過房屋總價的貸款金額、超過 100% 的貸款成數）

### Key Entities *(include if feature involves data)*

- **LoanInput**: 貸款輸入資料，包含房屋總價、貸款金額、貸款成數、自備款等屬性
- **RateStage**: 分段利率設定，包含期間（月數）和利率（百分比）

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
- [ ] Contract tests defined for API boundaries - N/A：本功能僅涉及 UI 元件，無 API 邊界
- [x] All tests will be written BEFORE implementation

#### User Experience (Constitution III)
- [x] UI components follow design system or have consistency plan
- [x] Accessibility requirements defined (WCAG 2.1 AA)
- [x] Responsive design requirements specified
- [x] Error messages are user-friendly and actionable
- [x] Loading states and async feedback mechanisms planned
- N/A：本功能為即時計算，無非同步載入狀態

#### Performance (Constitution IV)
- [x] Response time targets defined (API: <200ms, UI: FCP <1.5s, TTI <3.5s)
- [x] Performance budget set (bundle size, images, memory)
- [x] Performance testing strategy planned
- [x] Monitoring and observability considered
- [x] Scalability requirements assessed - N/A：本功能為純前端計算，無延展性考量

#### Documentation Language (Constitution V - NON-NEGOTIABLE)
- [x] This specification written in Traditional Chinese
- [x] User-facing documentation will be in Traditional Chinese
- [x] Code comments will be in English (business logic may use Traditional Chinese)
- [x] Internal communication will use Traditional Chinese

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 使用者在「使用房價與成數計算」模式下，100% 的欄位編輯操作能在 100 毫秒內完成同步計算並更新畫面
- **SC-002**: 分段利率調整時，100% 的增減操作顯示正確的兩位小數精度（如 2.01%），無浮點數誤差
- **SC-003**: 「使用房價與成數計算」開關的誤觸率降低至 0%（只有點擊開關本身才會觸發）
- **SC-004**: 使用者完成房貸試算的任務成功率維持或提升至 95% 以上

## Assumptions

- 使用者已理解「使用房價與成數計算」模式的用途
- 房屋總價為基準值，當使用者修改貸款成數、自備款或貸款金額時，房屋總價不會被自動修改
- 利率精度要求為小數點後兩位（0.01%）
- 現有 MudBlazor 元件庫支援自訂 Step 值和 ReadOnly 屬性的動態控制
