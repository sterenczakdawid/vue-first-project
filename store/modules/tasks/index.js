import tasks from "data/tasks.json";
export const state = () => ({
  tasks: tasks,
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
