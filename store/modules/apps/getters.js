export default {
  apps(state) {
    return state.apps;
  },
  hasTasks(state) {
    return state.apps && state.apps.length > 0;
  },
  headers(state) {
    return state.headers;
  },
  nextId: (state) => {
    const ids = state.apps.map((app) => parseInt(app.id, 10));
    return ids.length ? Math.max(...ids) + 1 : 1;
  },
};
