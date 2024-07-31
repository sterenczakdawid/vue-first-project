import axios from "axios";

export default {
  addApp(context, app) {
    const appData = {
      id: context.getters.nextId,
      name: app.name,
      created: app.created,
      edited: app.edited,
      serverId: app.serverId,
    };
    context.commit("addApp", appData);
    context.commit(
      "modules/tasks/attachTasksToApp",
      { id: appData.id, tasksIds: app.tasksIds },
      { root: true }
    );
  },
  updateApp(context, appData) {
    const { index, item } = appData;
    context.commit("updateApp", { index, item });
    context.commit("modules/tasks/detachTasksFromApp", item.id, { root: true });
    context.commit("modules/tasks/attachTasksToApp", item, { root: true });
  },
  removeApp(context, appId) {
    context.commit("removeApp", parseInt(appId));
  },
  removeServerApps(context, serverId) {
    context.commit("removeServerApps", serverId);
  },
  async loadApps(context) {
    const response = await axios.get("https://localhost:7233/api/App");
    // console.log(response.data);
    context.commit("setApps", response.data);
  },
};
