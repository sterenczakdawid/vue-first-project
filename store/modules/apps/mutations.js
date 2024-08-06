export default {
  addApp(state, app) {
    state.apps.push(app);
  },
  updateApp(state, { index, item }) {
    state.apps.splice(index, 1, item);
  },
  removeApp(state, appId) {
    const appIndex = state.apps.findIndex((app) => app.id === appId);
    state.apps.splice(appIndex, 1);
  },
  removeServerApps(state, serverId) {
    state.apps = state.apps.filter((app) => app.serverId != serverId);
  },
  setApps(state, payload) {
    state.apps = payload;
  },
  setTotalApps(state, payload) {
    state.totalApps = payload;
  },
};
