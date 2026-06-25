# Medicine Inventory

A single-page application for ABC Pharmacy to track medicines and record sales. Built with a .NET Core Web API backend and a React frontend, with data stored as JSON files on the server (no database).

## Features

- View all medicines in a grid (Name, Expiry Date, Quantity, Price, Brand — Notes is intentionally excluded).
- Color indications in the grid:
  - Red row when the expiry date is less than 30 days away.
  - Yellow row when quantity in stock is less than 10.
  - Expiry/red takes precedence when both rules apply.
- Add a new medicine via a validated form.
- Sell a medicine — reduces stock and records the sale.
- Sales history view (the "maintain sale records" requirement).
- Search medicines by name.

## Project structure

    MedicineInventory/
    ├── server/          .NET Core Web API
    │   ├── Controllers/     HTTP endpoints (medicines, sales)
    │   ├── Services/        business rules (validation, sell logic)
    │   ├── Repositories/    JSON file persistence
    │   ├── Models/          Medicine, Sale
    │   └── Data/            medicines.json, sales.json
    └── web/             React frontend (Vite)
        └── src/
            ├── api.js           backend calls
            ├── rules.js         grid color logic (unit tested)
            ├── App.jsx          main component
            └── components/      AddMedicineForm, MedicineGrid, SalesHistory

## Running the app

You need the .NET 8 SDK and Node.js installed.

### 1. Backend (terminal 1)

    cd server
    dotnet run

Note the URL it prints, e.g. `Now listening on: http://localhost:5067`.

### 2. Frontend (terminal 2)

    cd web
    npm install
    npm run dev

Open the printed URL (e.g. http://localhost:5173).

If the backend port is not 5067, update the one line in `web/src/api.js`:
`const API = "http://localhost:<your-port>/api";`

## API endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET  | /api/medicines?search= | List medicines (optional name search) |
| POST | /api/medicines | Add a medicine |
| POST | /api/medicines/{id}/sell | Sell (reduce stock + record sale) |
| GET  | /api/sales | Sales history, newest first |

## Tests

The grid color logic in `web/src/rules.js` is unit tested.

    cd web
    npm install -D vitest
    npm test

## Design decisions

- Layered backend — Controller to Service to Repository. The repository handles only persistence (the JSON file); the service holds business rules. Swapping JSON for a real database would mean writing one new repository class and changing one line in Program.cs — the service and controller are untouched.
- JSON file storage as required by the brief. Writes are guarded by a SemaphoreSlim lock so concurrent requests cannot corrupt the file.
- Sales as their own model/store so the system keeps a real transaction history, not just a running stock count.
- CORS is open (AllowAnyOrigin) for the dev build; in production it would be restricted to the known frontend origin.
- Color rule lives in its own module (rules.js) so it is easy to read and test.

## Possible next steps (with more time)

- Replace JSON storage with a real database (e.g. EF Core + SQLite/SQL Server).
- Edit / delete medicines.
- Authentication and roles (pharmacist vs admin).
- Pagination and sorting on the grid.
- Richer sales reporting (totals by day, low-stock alerts).
