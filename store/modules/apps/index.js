import apps from "data/applications.json";
export const state = () => ({
  apps: apps,
  headers: [
    {
      text: "ID",
      align: "start",
      sortable: false,
      value: "id",
    },
    { text: "Name", value: "name" },
    { text: "Created", value: "created" },
    { text: "Edited", value: "edited" },
  ],
});
