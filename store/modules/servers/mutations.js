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
};
