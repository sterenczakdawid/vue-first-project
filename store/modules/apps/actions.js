import axios from "axios";

export default {
  async addApp(context, app) {
    const appData = {
      name: app.name,
      created: app.created,
      edited: app.edited,
      serverId: app.serverId,
    };
    try {
      const res = await axios.post("https://localhost:7233/api/App", {
        ...appData,
        tasksIds: app.tasksIds,
      });
      const newAppId = res.data[res.data.length - 1].id;
      context.commit("addApp", { ...appData, id: newAppId });
      context.commit(
        "modules/tasks/attachTasksToApp",
        { id: newAppId, tasksIds: app.tasksIds },
        { root: true }
      );
      await context.dispatch("loadApps");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async updateApp(context, appData) {
    const { index, item } = appData;
    try {
      await axios.put("https://localhost:7233/api/App", item);

      context.commit("updateApp", { index, item });
      context.commit("modules/tasks/detachTasksFromApp", item.id, {
        root: true,
      });
      context.commit("modules/tasks/attachTasksToApp", item, { root: true });
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async removeApp(context, appId) {
    try {
      await axios.delete("https://localhost:7233/api/App", {
        params: { id: parseInt(appId) },
      });
      context.commit("removeApp", parseInt(appId));
      await context.dispatch("loadApps");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  removeServerApps(context, serverId) {
    context.commit("removeServerApps", serverId);
  },
  async loadApps(context, params) {
    const response = await axios.get("https://localhost:7233/api/App", {
      params,
    });
    context.commit("setApps", response.data);
    context.commit("setTotalApps", parseInt(response.headers["x-total-count"]));
  },
};
