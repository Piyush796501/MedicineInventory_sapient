import { useState } from "react";
import { addMedicine } from "../api";

const EMPTY = { fullName: "", notes: "", expiryDate: "", quantity: "", price: "", brand: "" };

export default function AddMedicineForm({ onAdded }) {
  const [form, setForm] = useState(EMPTY);
  const [err, setErr] = useState("");

  const set = (key) => (e) => setForm({ ...form, [key]: e.target.value });

  const submit = async (e) => {
    e.preventDefault();
    setErr("");
    if (!form.fullName.trim()) return setErr("Name is required.");
    if (form.quantity === "" || Number(form.quantity) < 0) return setErr("Quantity must be 0 or more.");
    if (form.price === "" || Number(form.price) < 0) return setErr("Price must be 0 or more.");
    if (!form.expiryDate) return setErr("Expiry date is required.");

    const res = await addMedicine({
      fullName: form.fullName.trim(),
      notes: form.notes,
      expiryDate: form.expiryDate,
      quantity: Number(form.quantity),
      price: Number(form.price),
      brand: form.brand,
    });
    if (!res.ok) return setErr(await res.text());
    setForm(EMPTY);
    onAdded();
  };

  return (
    <form className="card" onSubmit={submit}>
      <h3 style={{ marginTop: 0 }}>Add Medicine</h3>
      <div className="row">
        <label>Full Name<input value={form.fullName} onChange={set("fullName")} /></label>
        <label>Brand<input value={form.brand} onChange={set("brand")} /></label>
        <label>Expiry Date<input type="date" value={form.expiryDate} onChange={set("expiryDate")} /></label>
        <label>Quantity<input type="number" min="0" value={form.quantity} onChange={set("quantity")} /></label>
        <label>Price<input type="number" min="0" step="0.01" value={form.price} onChange={set("price")} /></label>
        <label style={{ flex: 1, minWidth: 180 }}>Notes<input value={form.notes} onChange={set("notes")} /></label>
        <button type="submit">Add</button>
      </div>
      {err && <div className="err">{err}</div>}
    </form>
  );
}
