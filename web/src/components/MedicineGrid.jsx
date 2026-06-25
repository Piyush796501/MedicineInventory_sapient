import { useState } from "react";
import { rowStatus } from "../rules";

function GridRow({ m, onSell }) {
  const [qty, setQty] = useState(1);
  return (
    <tr className={rowStatus(m)}>
      <td>{m.fullName}</td>
      <td>{new Date(m.expiryDate).toLocaleDateString()}</td>
      <td>{m.quantity}</td>
      <td>{Number(m.price).toFixed(2)}</td>
      <td>{m.brand}</td>
      <td>
        <input
          className="qty-input"
          type="number"
          min="1"
          value={qty}
          onChange={(e) => setQty(Number(e.target.value))}
        />
        <button style={{ marginLeft: 6 }} onClick={() => onSell(m.id, qty)}>Sell</button>
      </td>
    </tr>
  );
}

export default function MedicineGrid({ medicines, onSell }) {
  if (!medicines.length) return <p>No medicines found.</p>;
  return (
    <table>
      <thead>
        <tr>
          <th>Name</th><th>Expiry Date</th><th>Quantity</th><th>Price</th><th>Brand</th><th>Sell</th>
        </tr>
      </thead>
      <tbody>
        {medicines.map((m) => <GridRow key={m.id} m={m} onSell={onSell} />)}
      </tbody>
    </table>
  );
}
