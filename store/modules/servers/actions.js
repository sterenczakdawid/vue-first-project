export default {
  addServer({ commit, getters }, server) {
    server.id = getters.nextId.toString(); // Generowanie nowego ID
    commit("addServer", server);
  },
};
