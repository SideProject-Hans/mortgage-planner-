# Feature Specification: 貸款成數與圖表 UX 優化

**Feature Branch**: `004-ltv-chart-ux`  
**Created**: 2025-12-09  
**Status**: Draft  
**Input**: User description: "1. 貸款成數，增減的級數，應該是 1，不是 5。2. 還款趨勢圖，期數太多了，會看不清楚，可以減少顯示的期數"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - 精細調整貸款成數 (Priority: P1)

當使用者使用「房價與成數計算」模式時，需要能夠精確調整貸款成數，以反映實際銀行核貸的成數。銀行核貸成數通常以 1% 為單位，例如 70%、71%、72% 等，而非僅限於 70%、75%、80% 這類 5% 間隔的選項。

**Why this priority**: 這是基本功能的可用性問題，直接影響使用者計算的精確度。使用者無法精確輸入實際核貸成數會導致計算結果失準。

**Independent Test**: 使用者在貸款成數欄位點擊增加或減少按鈕，每次變動 1%，可獨立驗證此功能。

**Acceptance Scenarios**:

1. **Given** 使用者已開啟「使用房價與成數計算」模式，**When** 使用者點擊貸款成數欄位的增加按鈕，**Then** 貸款成數增加 1%
2. **Given** 使用者已開啟「使用房價與成數計算」模式，**When** 使用者點擊貸款成數欄位的減少按鈕，**Then** 貸款成數減少 1%
3. **Given** 貸款成數為 70%，**When** 使用者點擊增加按鈕三次，**Then** 貸款成數變為 73%
4. **Given** 貸款成數為 100%，**When** 使用者點擊增加按鈕，**Then** 貸款成數維持 100%（不超過上限）

---

### User Story 2 - 簡化還款趨勢圖顯示 (Priority: P1)

當使用者查看還款趨勢圖時，圖表應該清晰易讀。目前圖表每 12 期（每年）取樣一次，對於 30 年期貸款會有 30 個以上的資料點，加上 X 軸標籤「第 XX 期」過長，導致圖表擁擠難以閱讀。

**Why this priority**: 圖表是幫助使用者理解還款趨勢的重要視覺化工具，若無法清楚呈現，會降低整體使用體驗。

**Independent Test**: 使用者進行 30 年期貸款試算後，查看還款趨勢圖，圖表應清晰顯示主要趨勢，標籤應簡潔易讀。

**Acceptance Scenarios**:

1. **Given** 使用者已完成 30 年期貸款試算，**When** 使用者查看還款趨勢圖，**Then** 圖表顯示不超過 10 個資料點
2. **Given** 使用者已完成貸款試算，**When** 使用者查看還款趨勢圖，**Then** X 軸標籤以簡潔格式顯示（例如「Y1」、「Y5」、「Y10」代表年份）
3. **Given** 使用者已完成 10 年期貸款試算，**When** 使用者查看還款趨勢圖，**Then** 圖表以適當間隔顯示資料點，確保清晰呈現

---

### Edge Cases

- 當貸款年限少於 5 年時，圖表如何呈現？
- 當貸款成數已達邊界值（0% 或 100%）時，如何處理增減操作？
- 當使用者直接輸入貸款成數（非使用按鈕）時，系統如何處理？

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: 系統必須將貸款成數欄位的增減步進值 (Step) 設定為 1
- **FR-002**: 系統必須限制貸款成數在 0 到 100 的範圍內
- **FR-003**: 系統必須在還款趨勢圖中以年為單位進行資料取樣，間隔為 5 年
- **FR-004**: 系統必須將 X 軸標籤簡化為簡潔的年份格式（如「Y1」、「Y5」）
- **FR-005**: 系統必須限制還款趨勢圖的資料點數量，確保圖表清晰可讀（最多 10 個資料點）

### Key Entities

- **LoanInput**: 包含貸款成數 (LoanPercentage) 屬性，調整範圍 0-100
- **ChartSeries**: 圖表資料系列，控制顯示的資料點數量
- **XAxisLabels**: X 軸標籤陣列，控制標籤格式與間隔

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
- [x] Contract tests defined for API boundaries
- [x] All tests will be written BEFORE implementation

#### User Experience (Constitution III)
- [x] UI components follow design system or have consistency plan
- [x] Accessibility requirements defined (WCAG 2.1 AA)
- [x] Responsive design requirements specified
- [x] Error messages are user-friendly and actionable
- [x] Loading states and async feedback mechanisms planned

#### Performance (Constitution IV)
- [x] Response time targets defined (API: <200ms, UI: FCP <1.5s, TTI <3.5s)
- [x] Performance budget set (bundle size, images, memory)
- [x] Performance testing strategy planned
- [x] Monitoring and observability considered
- [x] Scalability requirements assessed

#### Documentation Language (Constitution V - NON-NEGOTIABLE)
- [x] This specification written in Traditional Chinese
- [x] User-facing documentation will be in Traditional Chinese
- [x] Code comments will be in English (business logic may use Traditional Chinese)
- [x] Internal communication will use Traditional Chinese

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: 使用者能在 3 秒內將貸款成數從 70% 調整到 73%
- **SC-002**: 30 年期貸款的還款趨勢圖資料點數量不超過 10 個
- **SC-003**: 還款趨勢圖的 X 軸標籤長度不超過 3 個字元
- **SC-004**: 使用者首次查看還款趨勢圖即能理解整體還款走勢，無需額外解釋

## Assumptions

- 使用者了解貸款成數代表貸款金額佔房屋總價的百分比
- 現有的 MudBlazor 圖表元件支援自訂 X 軸標籤格式
- 以 5 年為間隔取樣不會影響使用者對還款趨勢的理解
