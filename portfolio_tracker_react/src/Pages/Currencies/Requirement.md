# Exchange Rates Page Requirements

## Overview

The page should display exchange rates between currencies over time using D3.js to render a line chart.

## Data Requirements

1. The page must fetch currency exchange rate data using queryCurrencyExchangeRates method
2. Required API parameters:
   - startDate: Initial date of the range
   - endDate: Final date of the range
   - baseCurrency: Base currency for conversion
   - targetCurrency: Target currency for conversion

## UI Requirements

1. Display a line chart with:
   - X-axis showing dates (daily intervals)
   - Y-axis showing exchange rates (base -> target)
   - Proper axis labels
   - Interactive tooltip on hover
   - Responsive design within container
2. Add input controls at the top for:
   - Selecting base currency
   - Selecting target currency
   - Date range selection

## Additional Requirements

1. Error handling:
   - Display error messages if data fetch fails
   - Handle invalid date inputs
2. Loading state:
   - Show loading indicator while fetching data
3. Empty state:
   - Display message if no rate data is available
