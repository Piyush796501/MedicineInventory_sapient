// Business rule for grid row coloring — kept separate so it is easy to test.
export const MS_PER_DAY = 86_400_000;

export function daysUntil(dateStr, now = new Date()) {
  return Math.ceil((new Date(dateStr) - now) / MS_PER_DAY);
}

// "expiring" (red) takes precedence over "low-stock" (yellow).
export function rowStatus(medicine, now = new Date()) {
  if (daysUntil(medicine.expiryDate, now) < 30) return "expiring";
  if (medicine.quantity < 10) return "low-stock";
  return "";
}
