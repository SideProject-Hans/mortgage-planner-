# Research: 房屋貸款試算工具

**Branch**: `001-mortgage-calculator` | **Date**: 2025-12-09
**Spec**: [spec.md](./spec.md)

## 1. Unknowns & Clarifications

| Unknown | Status | Resolution |
|---------|--------|------------|
| Tech Stack | Resolved | Confirmed C# Blazor WASM + MudBlazor + LocalStorage |
| Chart Library | Resolved | MudBlazor Charts (MudChart) |
| Storage Limit | Resolved | LocalStorage (5-10MB limit is sufficient for text history) |

## 2. Technology Selection

### Framework: Blazor WebAssembly (.NET 8)

- **Decision**: Use Blazor WASM.
- **Rationale**: Allows sharing C# logic for loan calculations. Runs entirely in the browser (Static Web App), reducing hosting costs and latency.
- **Alternatives**: React/Vue (Requires JS interop or rewriting logic), Blazor Server (Requires active connection, higher latency).

### UI Library: MudBlazor

- **Decision**: Use MudBlazor.
- **Rationale**: Provides a comprehensive set of Material Design components, including Grid, Inputs, and Charts, ensuring a consistent look and feel.
- **Alternatives**: Radzen, Blazorise (MudBlazor chosen for better Material Design implementation and free chart components).

### Storage: Blazored.LocalStorage

- **Decision**: Use Blazored.LocalStorage.
- **Rationale**: Simple API for accessing browser LocalStorage. Sufficient for storing user's calculation history (JSON).
- **Alternatives**: IndexedDB (Overkill for simple history), SQLite WASM (Too heavy).

## 3. Best Practices & Patterns

### MudBlazor Charts (MudChart) Performance

- **Update Frequency:** Throttle updates to a reasonable frame rate (e.g., 500ms-1000ms) rather than updating on every single data point change.
- **Control Rendering:** Use `@key` on chart elements or override `ShouldRender` in a wrapping component to prevent the chart from re-rendering when unrelated page state changes.
- **Data Points:** Keep the number of data points reasonable. Aggregate data if necessary.
- **Reactivity:** Explicitly call `StateHasChanged()` if data changes but the chart doesn't update.

### Blazored.LocalStorage Usage

- **Synchronous vs. Asynchronous:** **Always prefer Asynchronous (`SetItemAsync`, `GetItemAsync`)** to avoid blocking the UI thread.
- **Data Size Limits:** Be aware of the 5-10MB browser limit. Avoid storing massive datasets.

### General Blazor WebAssembly Dashboard Performance

- **Render Tree Optimization (`ShouldRender`):** Override `ShouldRender()` in "Widget" components to isolate updates.
- **Parameter Types:** Use primitive types or immutable records for component parameters to speed up diffing.
- **Virtualization:** Use `<Virtualize>` for long lists (e.g., amortization schedule).
- **AOT Compilation:** Consider enabling `<RunAOTCompilation>true</RunAOTCompilation>` for better runtime performance, trading off initial load size.
- **Release Build:** Always test performance in `Release` mode.
