export default {
  addServer(state, server) {
    state.servers.push(server);
  },
  removeServer(state, serverId) {
    const serverIndex = state.servers.findIndex(
      (server) => server.id === serverId
    );
    state.servers.splice(serverIndex, 1);
  },
  updateServer(state, { index, item }) {
    state.servers.splice(index, 1, item);
  },
  setServers(state, payload) {
    state.servers = payload;
  },
  setTotalServers(state, payload) {
    state.totalServers = payload;
  },
};
