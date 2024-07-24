export default {
  addServer(state, server) {
    state.servers.push(server);
  },
  removeServer(state, serverId) {
    const serverIndex = state.servers.findIndex(
      (server) => server.id === serverId
    );
    // console.log(typeof serverId, typeof serverIndex);
    state.servers.splice(serverIndex, 1);
  },
  updateServer(state, { index, item }) {
    console.log("otrzymalem ", { index, item });
    // state.servers[serverIndex].name = server.name;
    state.servers.splice(index, 1, item);
  },
};
