import { useEffect, useState } from "react";
import { getMedicines, sellMedicine, getSales } from "./api";
import AddMedicineForm from "./components/AddMedicineForm";
import MedicineGrid from "./components/MedicineGrid";
import SalesHistory from "./components/SalesHistory";
import "./App.css";

export default function App() {
  const [medicines, setMedicines] = useState([]);
  const [sales, setSales] = useState([]);
  const [search, setSearch] = useState("");
  const [view, setView] = useState("inventory"); // "inventory" | "sales"
  const [error, setError] = useState("");

  const loadInventory = async (q = search) => {
    try {
      setMedicines(await getMedicines(q));
      setError("");
    } catch {
      setError("Cannot reach the API. Is the backend running?");
    }
  };

  const loadSales = async () => {
    try { setSales(await getSales()); } catch { /* ignore */ }
  };

  useEffect(() => { loadInventory(""); }, []);

  const handleSell = async (id, qty) => {
    const res = await sellMedicine(id, qty);
    if (!res.ok) { alert(await res.text()); return; }
    loadInventory();
    if (view === "sales") loadSales();
  };

  const openSales = () => { setView("sales"); loadSales(); };

  return (
    <div className="app">
      <h1>Medicine Inventory</h1>
      <div className="sub">ABC Pharmacy — stock &amp; sales</div>

      <div className="toolbar">
        <div>
          <button className={view === "inventory" ? "" : "secondary"}
            onClick={() => setView("inventory")}>Inventory</button>
          <button style={{ marginLeft: 8 }} className={view === "sales" ? "" : "secondary"}
            onClick={openSales}>Sales History</button>
        </div>

        {view === "inventory" && (
          <div>
            <input placeholder="Search by name…" value={search}
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => e.key === "Enter" && loadInventory()} />
            <button style={{ marginLeft: 6 }} onClick={() => loadInventory()}>Search</button>
            <button style={{ marginLeft: 6 }} className="secondary"
              onClick={() => { setSearch(""); loadInventory(""); }}>Clear</button>
          </div>
        )}
      </div>

      {error && <div className="err">{error}</div>}

      {view === "inventory" ? (
        <>
          <AddMedicineForm onAdded={() => loadInventory("")} />
          <div className="card">
            <div className="legend">
              <span><span className="swatch" style={{ background: "#f8d7da" }} />Expiring within 30 days</span>
              <span><span className="swatch" style={{ background: "#fff3cd" }} />Low stock (&lt; 10)</span>
            </div>
            <MedicineGrid medicines={medicines} onSell={handleSell} />
          </div>
        </>
      ) : (
        <div className="card"><SalesHistory sales={sales} /></div>
      )}
    </div>
  );
}
