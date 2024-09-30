export interface ContainerEntryRequest {
  longitude: number | null;
  latitude: number | null;
  containerID: number | null;
  dateInserted?: Date; // Optionally include this if needed; if not, server-side can set the date.
}
