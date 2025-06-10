# ImportTransaction Page Requirements

## Overview
The ImportTransaction page is designed to facilitate the import of transaction lists from a Scalable Capital bank account. The page will allow users to select a file containing transaction data, upload it to the `PortfolioTracker.Imports.WebApi` application, and display the candidate transactions in the `ImportTransactionsForm` React component.

## Features

### Header
- Header will be "Transactions - Import
- **Purpose:** Display the name of the bank account and the currency code.
- **Implementation:** Use the `BankAccountDetail` component to display the bank account name and currency code.

### File Selection Control
- **Purpose:** Allow users to select a transaction list file from a Scalable Capital bank account.
- **Implementation:** Use an HTML `<input type="file">` element to enable file selection within the ImportTransaction page.

### File Upload
- **Target Application:** `PortfolioTracker.Imports.WebApi`
- **New Endpoint:** Create a new endpoint named `DraftImportTransactionsFromFileCommandHandler`.
- **Functionality:**
  - Accept the uploaded file.
  - Return all candidate transaction items.
  - For "Buy Asset" or "Sell Asset" transaction types, resolve and include the corresponding `PortfolioId`.

### React Component Integration

#### ImportTransactionsForm Component
- **Input:** Receive a list of candidate transactions from the `DraftImportTransactionsFromFileCommandHandler` endpoint.
- **Display:** Render the list of transactions within the component.

## Technical Details

### Endpoint: DraftImportTransactionsFromFileCommandHandler
- **Route:** Define a POST route to handle file uploads.
- **Response:** Return a list of candidate transaction items, enriched with `PortfolioId` for applicable transaction types.

### ImportTransactionsForm Component
- **State Management:** Use React hooks (e.g., `useState`, `useEffect`) to manage and display the imported transaction data.
- **Rendering:** Use a table or list to display transaction details, allowing for easy review by the user.

## Notes
- Ensure the file input control is styled consistently with the application's design.
- Implement appropriate error handling for file uploads and API responses.
- Consider adding loading indicators while the file is being processed and transactions are being fetched.
