<!--
================================================================================
SYNC IMPACT REPORT
================================================================================
Version change: 0.0.0 → 1.0.0 (MAJOR: Initial constitution adoption)

Modified principles: N/A (new document)

Added sections:
  - Core Principles (4 principles: Code Quality, Testing Standards, UX Consistency, Performance Requirements)
  - Quality Gates section
  - Development Workflow section
  - Governance section

Removed sections: N/A

Templates requiring updates:
  ✅ plan-template.md - Constitution Check section references these principles
  ✅ spec-template.md - User Scenarios align with Testing Standards principle
  ✅ tasks-template.md - Task phases align with Quality Gates

Follow-up TODOs: None
================================================================================
-->

# Mortgage Planner Constitution

## Core Principles

### I. Code Quality First

**所有程式碼 MUST 遵循可維護性、可讀性和一致性標準。**

- **命名規範**: 變數、函式和類別 MUST 使用描述性且有意義的名稱，遵循專案語言的慣例（TypeScript: camelCase/PascalCase）
- **單一職責**: 每個函式、元件和模組 MUST 只負責一項明確定義的任務
- **程式碼重複禁止**: 違反 DRY (Don't Repeat Yourself) 原則的程式碼 MUST 重構為可重用元件或工具函式
- **型別安全**: TypeScript 專案 MUST 啟用 strict mode，禁止使用 `any` 類型（除非有明確書面理由）
- **錯誤處理**: 所有非同步操作和外部 API 呼叫 MUST 包含適當的錯誤處理和用戶回饋機制
- **文件註解**: 公開 API、複雜邏輯和非顯而易見的決策 MUST 包含說明性註解

**Rationale**: 房貸計算涉及財務敏感性資料，程式碼品質直接影響計算準確性和系統可靠性。

### II. Testing Standards (NON-NEGOTIABLE)

**測試是功能開發的前提條件，不是事後補充。**

- **TDD 執行週期**: 測試撰寫 → 確認測試失敗 (Red) → 實作功能 (Green) → 重構 (Refactor)
- **覆蓋率要求**:
  - 核心計算邏輯（房貸利率、還款計算）: MUST 達到 90%+ 覆蓋率
  - 業務邏輯層: MUST 達到 80%+ 覆蓋率
  - UI 元件: SHOULD 達到 70%+ 覆蓋率
- **測試類型分佈**:
  - Unit Tests: 所有純函式和計算邏輯
  - Integration Tests: API 端點和資料流
  - E2E Tests: 關鍵用戶旅程（申請流程、計算結果展示）
- **測試命名**: 測試 MUST 使用 `should_[預期行為]_when_[條件]` 格式命名
- **測試隔離**: 每個測試 MUST 獨立執行，不依賴其他測試的狀態或執行順序

**Rationale**: 財務應用程式的計算錯誤可能導致用戶做出錯誤的財務決策，測試是防止此類錯誤的最後防線。

### III. User Experience Consistency

**用戶體驗 MUST 保持一致、可預測且無障礙。**

- **設計系統遵循**: 所有 UI 元件 MUST 遵循統一的設計系統（色彩、間距、字體、動畫）
- **響應式設計**: 所有頁面 MUST 支援桌面 (1920px)、平板 (768px) 和行動裝置 (375px) 三種斷點
- **無障礙標準**:
  - MUST 符合 WCAG 2.1 AA 級標準
  - 所有互動元素 MUST 支援鍵盤導航
  - 色彩對比度 MUST 達到 4.5:1（一般文字）或 3:1（大型文字）
- **狀態回饋**: 所有用戶操作 MUST 在 100ms 內提供視覺回饋（loading 狀態、按鈕狀態變化）
- **錯誤訊息**: 錯誤訊息 MUST 使用人類可讀的語言，並提供解決方案建議
- **表單驗證**: 即時驗證 MUST 在用戶完成欄位輸入後立即執行，不等待表單提交

**Rationale**: 房貸決策對用戶而言是重大財務決策，清晰一致的介面可降低用戶焦慮並建立信任。

### IV. Performance Requirements

**應用程式效能 MUST 符合既定指標，確保流暢的用戶體驗。**

- **Core Web Vitals 目標**:
  - Largest Contentful Paint (LCP): MUST < 2.5 秒
  - First Input Delay (FID): MUST < 100 毫秒
  - Cumulative Layout Shift (CLS): MUST < 0.1
- **Bundle Size 限制**:
  - 初始載入 JavaScript: MUST < 200KB (gzipped)
  - 首頁總資源: MUST < 500KB (gzipped)
- **計算效能**:
  - 單次房貸計算: MUST < 50ms
  - 批次計算（比較方案）: MUST < 200ms
- **API 回應時間**:
  - P95 回應時間: MUST < 500ms
  - P99 回應時間: MUST < 1000ms
- **記憶體使用**: 前端應用程式 MUST 不產生記憶體洩漏，長時間使用後記憶體使用量 MUST 穩定
- **離線支援**: 核心計算功能 SHOULD 支援離線使用（Service Worker 快取）

**Rationale**: 效能直接影響用戶留存率和轉換率，尤其在行動裝置上更為關鍵。

## Quality Gates

**所有程式碼變更 MUST 通過以下品質關卡才能合併：**

1. **Pre-commit**:
   - Linting (ESLint/Prettier): 零錯誤
   - Type checking: 零型別錯誤
   - Unit tests: 所有測試通過

2. **Pull Request**:
   - Code review: 至少一位審核者批准
   - All CI checks: 通過
   - Test coverage: 不得低於現有覆蓋率
   - Bundle size: 不得超過限制

3. **Pre-deploy**:
   - Integration tests: 全部通過
   - E2E tests: 關鍵路徑全部通過
   - Performance audit: Lighthouse 分數 > 90

## Development Workflow

**開發流程 MUST 遵循以下規範：**

1. **分支策略**:
   - `main`: 生產環境程式碼，僅接受 PR 合併
   - `develop`: 開發整合分支
   - `feature/*`: 功能開發分支
   - `fix/*`: 錯誤修復分支

2. **Commit 規範**: 遵循 Conventional Commits 格式
   - `feat:` 新功能
   - `fix:` 錯誤修復
   - `docs:` 文件更新
   - `refactor:` 重構（不改變功能）
   - `test:` 測試新增或修改
   - `perf:` 效能改進

3. **Code Review 要求**:
   - 所有 PR MUST 包含變更說明和測試證據
   - 審核者 MUST 驗證程式碼符合 Constitution 原則
   - 複雜度增加 MUST 提供書面理由

## Governance

**本 Constitution 為專案最高指導原則，所有實踐 MUST 遵循本文件規範。**

- **優先順序**: Constitution > 團隊慣例 > 個人偏好
- **修訂流程**:
  1. 提出修訂提案（PR 至 `.specify/memory/constitution.md`）
  2. 團隊討論與投票（需 2/3 多數同意）
  3. 更新版本號並記錄變更理由
  4. 同步更新所有受影響的模板和文件
- **版本規則**:
  - MAJOR: 原則移除或重新定義（破壞性變更）
  - MINOR: 新增原則或重大擴充
  - PATCH: 澄清、措辭或錯字修正
- **合規審查**: 每季度進行一次 Constitution 合規審查，確保實際執行與規範一致

**Version**: 1.0.0 | **Ratified**: 2025-12-05 | **Last Amended**: 2025-12-05
