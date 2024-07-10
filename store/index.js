export const state = () => ({
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
    { text: "Actions", value: "actions", sortable: false },
  ],
});

export const getters = {
  getHeaders(state) {
    return state.headers;
  },
};
