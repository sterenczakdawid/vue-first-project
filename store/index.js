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
  ],
});

export const getters = {
  getHeaders(state) {
    return state.headers;
  },
};
