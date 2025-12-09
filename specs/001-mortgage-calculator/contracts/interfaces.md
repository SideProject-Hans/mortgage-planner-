# Contracts: Service Interfaces

**Branch**: `001-mortgage-calculator` | **Date**: 2025-12-09

## ICalculationService

負責執行貸款計算的核心邏輯。

```csharp
public interface ICalculationService
{
    /// <summary>
    /// 計算貸款結果
    /// </summary>
    /// <param name="input">貸款輸入參數</param>
    /// <returns>計算結果</returns>
    LoanResult Calculate(LoanInput input);

    /// <summary>
    /// 驗證輸入參數是否有效
    /// </summary>
    /// <param name="input">貸款輸入參數</param>
    /// <returns>驗證結果 (true: 有效, false: 無效)</returns>
    bool Validate(LoanInput input, out List<string> errors);
}
```

## IStorageService

負責資料的持久化 (LocalStorage)。

```csharp
public interface IStorageService
{
    /// <summary>
    /// 儲存試算記錄
    /// </summary>
    /// <param name="calculation">試算記錄</param>
    Task SaveCalculationAsync(SavedCalculation calculation);

    /// <summary>
    /// 取得所有儲存的試算記錄
    /// </summary>
    Task<List<SavedCalculation>> GetAllCalculationsAsync();

    /// <summary>
    /// 刪除試算記錄
    /// </summary>
    /// <param name="id">記錄 ID</param>
    Task DeleteCalculationAsync(Guid id);

    /// <summary>
    /// 取得單一試算記錄
    /// </summary>
    Task<SavedCalculation?> GetCalculationAsync(Guid id);
}
```
