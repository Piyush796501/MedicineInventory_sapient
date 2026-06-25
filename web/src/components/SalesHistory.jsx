export default function SalesHistory({ sales }) {
  if (!sales.length) return <p>No sales recorded yet.</p>;
  return (
    <table>
      <thead>
        <tr><th>Medicine</th><th>Qty</th><th>Unit Price</th><th>Total</th><th>Sold At</th></tr>
      </thead>
      <tbody>
        {sales.map((s) => (
          <tr key={s.id}>
            <td>{s.medicineName}</td>
            <td>{s.quantity}</td>
            <td>{Number(s.unitPrice).toFixed(2)}</td>
            <td>{Number(s.totalPrice).toFixed(2)}</td>
            <td>{new Date(s.soldAt).toLocaleString()}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
