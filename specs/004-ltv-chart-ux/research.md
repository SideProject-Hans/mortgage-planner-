# Research: 貸款成數與圖表 UX 優化

**Feature**: 004-ltv-chart-ux  
**Date**: 2025-12-09  
**Status**: Complete

## Research Tasks

### 1. MudNumericField Step 屬性行為研究

**背景**: 需確認 MudBlazor `MudNumericField` 的 `Step` 屬性行為

**發現**:
- `Step` 屬性控制使用者點擊增減按鈕時的變動量
- 當前設定: `Step="5"` (貸款成數欄位，LoanInputForm.razor 第 17 行)
- 目標設定: `Step="1"`
- `Min` 和 `Max` 屬性已正確設定 (0 和 100)

**Decision**: 直接修改 `Step` 屬性值從 5 改為 1
**Rationale**: MudBlazor 元件 API 穩定，無需額外處理
**Alternatives considered**: 無

### 2. MudChart 取樣間隔研究

**背景**: 需了解現有圖表取樣邏輯並設計新的取樣策略

**現有實作分析** (CalculatorPage.razor):

```csharp
// UpdateChart() 方法 (單一試算模式)
var sampledSchedule = result.Schedule.Where((x, i) => i % 12 == 0 || i == result.Schedule.Count - 1).ToList();
XAxisLabels = sampledSchedule.Select(x => $"第{x.Period}期").ToArray();
```

- 現有取樣: 每 12 期 (每年) 取樣一次
- 30 年期貸款: 約 31 個資料點 (過多)
- X 軸標籤: "第X期" (過長)

```csharp
// UpdateComparisonChart() 方法 (方案比較模式)
for (int i = 0; i <= years; i += 5) // Label every 5 years
{
    labels.Add($"Y{i}");
}
```

- 比較模式已使用 5 年間隔和 "Y{n}" 格式
- 可參考此設計套用到單一試算模式

**Decision**: 採用與比較模式一致的取樣策略
**Rationale**: 
1. 保持 UI 一致性
2. 5 年間隔已在比較模式驗證有效
3. "Y{n}" 標籤格式簡潔易讀

**Alternatives considered**:
- 每 3 年取樣: 10 年期貸款仍有 4 個點，但 30 年期有 11 個點，略多
- 動態計算間隔: 增加複雜度，不符合最小化原則

### 3. 短期貸款特殊處理研究

**背景**: 規格要求短期貸款 (≤5年) 以每年取樣

**設計方案**:

```csharp
// 判斷取樣間隔
int sampleIntervalYears = years <= 5 ? 1 : 5;
```

**Decision**: 使用條件判斷選擇取樣間隔
**Rationale**: 
1. 邏輯簡單清晰
2. 確保短期貸款有足夠資料點 (3-5 年期有 4-6 個點)
3. 長期貸款維持清晰度 (30 年期有 7 個點)

**Alternatives considered**:
- 固定資料點數量再動態計算間隔: 增加複雜度
- 完全依據資料點數量判斷: 可能導致不一致的使用者體驗

## Technical Context Resolved

所有 NEEDS CLARIFICATION 項目已解決：

| 項目 | 解決方案 |
|------|---------|
| 步進值修改方式 | 修改 `Step` 屬性值 |
| 取樣間隔策略 | 長期 (>5年): 5 年間隔; 短期 (≤5年): 1 年間隔 |
| X 軸標籤格式 | "Y{n}" 格式 (如 Y0, Y5, Y10) |

## Implementation Notes

### 修改點 1: LoanInputForm.razor

```razor
<!-- 第 17 行: 修改 Step 屬性 -->
<MudNumericField ... Step="1" ... />
```

### 修改點 2: CalculatorPage.razor - UpdateChart()

```csharp
private void UpdateChart()
{
    if (result == null) return;

    var years = (int)Math.Ceiling(result.Schedule.Count / 12.0);
    int sampleIntervalYears = years <= 5 ? 1 : 5;
    
    var labels = new List<string>();
    var balanceData = new List<double>();
    var interestData = new List<double>();

    for (int year = 0; year <= years; year += sampleIntervalYears)
    {
        labels.Add($"Y{year}");
        // ... 取得對應年份的資料 ...
    }

    XAxisLabels = labels.ToArray();
    Series = new List<ChartSeries>() { ... };
}
```

## Conclusion

研究完成，無額外技術障礙。可進入 Phase 1 設計階段。
