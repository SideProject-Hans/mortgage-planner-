# Data Model: 房屋貸款試算工具

**Branch**: `001-mortgage-calculator` | **Date**: 2025-12-09
**Spec**: [spec.md](./spec.md)

## Entities

### LoanInput (貸款輸入)

使用者輸入的貸款參數。

| Field | Type | Description | Validation |
|-------|------|-------------|------------|
| Amount | decimal | 貸款金額 (萬元) | > 0, Required |
| TermYears | int | 貸款年限 (年) | > 0, <= 40, Required |
| InterestRate | decimal | 年利率 (%) | >= 0, Required |
| RepaymentType | enum | 還款方式 | EqualPrincipalAndInterest (本息平均), EqualPrincipal (本金平均) |
| GracePeriodYears | int | 寬限期 (年) | >= 0, < TermYears |
| StartDate | DateTime | 開始還款日期 | Required |

### LoanResult (試算結果)

計算後的匯總結果。

| Field | Type | Description |
|-------|------|-------------|
| MonthlyPayment | decimal | 首月還款金額 (元) |
| TotalPayment | decimal | 總還款金額 (元) |
| TotalInterest | decimal | 總利息支出 (元) |
| Schedule | List<AmortizationScheduleItem> | 還款明細表 |

### AmortizationScheduleItem (還款明細項目)

每一期的還款細節。

| Field | Type | Description |
|-------|------|-------------|
| Period | int | 期數 (月) |
| Date | DateTime | 還款日期 |
| Payment | decimal | 本期還款總額 |
| Principal | decimal | 本期償還本金 |
| Interest | decimal | 本期償還利息 |
| Balance | decimal | 本期剩餘本金 |

### SavedCalculation (儲存的試算)

儲存在 LocalStorage 的記錄。

| Field | Type | Description |
|-------|------|-------------|
| Id | Guid | 唯一識別碼 |
| Name | string | 試算名稱 (使用者自訂或自動產生) |
| CreatedAt | DateTime | 建立時間 |
| Input | LoanInput | 輸入參數 |
| Result | LoanResult | 計算結果 (可選，或重新計算) |

## Enums

### RepaymentType

- `EqualPrincipalAndInterest`: 本息平均攤還
- `EqualPrincipal`: 本金平均攤還

## State Management

- **CurrentCalculation**: 當前正在編輯/檢視的試算。
- **SavedCalculations**: 已儲存的試算列表。
- **ComparisonA**: 用於比較的方案 A。
- **ComparisonB**: 用於比較的方案 B。
