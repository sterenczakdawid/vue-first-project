export default {
  servers(state) {
    return state.servers;
  },
  hasServers(state) {
    return state.servers && state.servers.length > 0;
  },
  nextId: (state) => {
    const ids = state.servers.map((server) => parseInt(server.id, 10));
    return ids.length ? Math.max(...ids) + 1 : 1;
  },
};
