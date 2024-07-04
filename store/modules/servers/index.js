export const state = () => ({
  servers: [
    {
      id: "1",
      name: "server1",
      created: new Date(),
      edited: new Date(),
      apps: [1, 2, 3],
    },
    {
      id: "2",
      name: "server2",
      created: new Date(),
      edited: new Date(),
      apps: [1, 4],
    },
    {
      id: "3",
      name: "server3",
      created: new Date(),
      edited: new Date(),
      apps: [9],
    },
    {
      id: "4",
      name: "server4",
      created: new Date(),
      edited: new Date(),
      apps: [5],
    },
    {
      id: "5",
      name: "server5",
      created: new Date(),
      edited: new Date(),
      apps: [],
    },
    {
      id: "6",
      name: "server6",
      created: new Date(),
      edited: new Date(),
      apps: [5],
    },
    {
      id: "7",
      name: "server7",
      created: new Date(),
      edited: new Date(),
      apps: [4],
    },
  ],
  headers: [
    {
      text: "ID",
      align: "start",
      sortable: false,
      value: "name",
    },
    { text: "Name", value: "name" },
    { text: "Created", value: "created" },
    { text: "Edited", value: "edited" },
  ],
});
