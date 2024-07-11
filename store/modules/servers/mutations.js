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
    console.log("otrzymalem ", { index, item });
    // state.servers[serverIndex].name = server.name;
    state.servers.splice(index, 1, item);
  },
  // updateServer(state, { id, item }) {
  //   const index = state.servers.findIndex((server) => server.id === id);
  //   if (index !== -1) {
  //     state.servers.splice(index, 1, item);
  //   }
  // },
};
