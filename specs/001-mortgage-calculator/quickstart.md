# Quickstart: 房屋貸款試算工具

**Branch**: `001-mortgage-calculator` | **Date**: 2025-12-09

## Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or VS Code (with C# Dev Kit)

## Setup

1. Clone the repository.
2. Navigate to the project root.
3. Restore dependencies:

   ```bash
   dotnet restore
   ```

## Running the Application

1. Navigate to the Blazor project directory:

   ```bash
   cd src/MortgageCalculator
   ```

2. Run the application:

   ```bash
   dotnet watch run
   ```

3. Open your browser and navigate to `http://localhost:5000` (or the port shown in the terminal).

## Running Tests

1. Navigate to the test project directory:

   ```bash
   cd tests/MortgageCalculator.Tests
   ```

2. Run tests:

   ```bash
   dotnet test
   ```

## Troubleshooting

### Issue: MudBlazor Component Rendering Errors (2025-12-09)

**錯誤訊息**:
```
Missing <MudPopoverProvider />, please add it to your layout.
Could not find 'mudElementRef.getBoundingClientRect' ('mudElementRef' was undefined).
Could not find 'mudElementRef.addOnBlurEvent' ('mudElementRef' was undefined).
```

**問題原因**:
MudBlazor 安裝不完整，缺少必要的 Providers 和前端資源引用。

**解決方案**:

1. **在 `MainLayout.razor` 加入所有必要的 Providers**:
   ```razor
   @inherits LayoutComponentBase

   <MudThemeProvider />
   <MudPopoverProvider />   <!-- 必須加入 -->
   <MudDialogProvider />
   <MudSnackbarProvider />
   ```

2. **在 `wwwroot/index.html` 加入 MudBlazor CSS 和 JS**:
   ```html
   <!-- 在 <head> 內加入 CSS -->
   <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
   <link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />

   <!-- 在 </body> 前加入 JS (在 blazor.webassembly.js 之後) -->
   <script src="_content/MudBlazor/MudBlazor.min.js"></script>
   ```

**相關文件**: [MudBlazor Installation Guide](https://mudblazor.com/getting-started/installation)
