export default {
  servers(state) {
    return state.servers;
  },
  totalServers(state) {
    return state.totalServers;
  },
  hasServers(state) {
    return state.servers && state.servers.length > 0;
  },
};
