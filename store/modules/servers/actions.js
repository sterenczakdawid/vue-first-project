import axios from "axios";

export default {
  async addServer(context, server) {
    const serverData = {
      // id: context.getters.nextId,
      name: server.name,
      created: server.created,
      edited: server.edited,
      // tasksIds: server.tasksIds,
      // appsIds: server.appsIds,
    };
    const res = await axios.post(
      "https://localhost:7233/api/Server",
      serverData
    );
    console.log(res.data[res.data.length - 1]);
    context.commit("addServer", {
      ...serverData,
      id: res.data[res.data.length - 1].id,
    });
  },
  async removeServer(context, serverId) {
    const res = await axios.delete("https://localhost:7233/api/Server", {
      params: { id: parseInt(serverId) },
    });
    console.log(res);
    context.commit("modules/tasks/removeServerTasks", parseInt(serverId), {
      root: true,
    });
    context.commit("modules/apps/removeServerApps", parseInt(serverId), {
      root: true,
    });
    context.commit("removeServer", parseInt(serverId));
  },
  updateServer(context, serverData) {
    const { index, item } = serverData;
    context.commit("updateServer", { index, item });
  },
  async loadServers(context) {
    const response = await axios.get("https://localhost:7233/api/Server");
    // console.log(response.data);
    context.commit("setServers", response.data);
  },
};
