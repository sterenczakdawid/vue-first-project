export default {
  servers(state) {
    return state.servers;
  },
  hasServers(state) {
    return state.servers && state.servers.length > 0;
  },
  headers(state) {
    return state.headers;
  },
};
