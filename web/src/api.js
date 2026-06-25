// All calls to the .NET backend live here.
// Change the port if your API prints a different one in its terminal.
const API = "http://localhost:5067/api";

export const getMedicines = (search = "") =>
  fetch(`${API}/medicines${search ? `?search=${encodeURIComponent(search)}` : ""}`)
    .then((r) => r.json());

export const addMedicine = (m) =>
  fetch(`${API}/medicines`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(m),
  });

export const sellMedicine = (id, quantity) =>
  fetch(`${API}/medicines/${id}/sell`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ quantity }),
  });

export const getSales = () => fetch(`${API}/sales`).then((r) => r.json());
