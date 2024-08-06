import axios from "axios";

export default {
  async addServer(context, server) {
    const serverData = {
      name: server.name,
      created: server.created,
      edited: server.edited,
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
    const res = await axios.delete(
      `https://localhost:7233/api/Task/server/${serverId}`
    );
    const res2 = await axios.delete("https://localhost:7233/api/Server", {
      params: { id: parseInt(serverId) },
    });
    const res3 = await axios.delete(
      `https://localhost:7233/api/App/server/${serverId}`
    );
    context.commit("modules/tasks/removeServerTasks", parseInt(serverId), {
      root: true,
    });
    context.commit("modules/apps/removeServerApps", parseInt(serverId), {
      root: true,
    });
    context.commit("removeServer", parseInt(serverId));
  },
  async updateServer(context, serverData) {
    const { index, item } = serverData;
    const res = await axios.put("https://localhost:7233/api/Server", item);
    context.commit("updateServer", { index, item });
  },
  async loadServers(context, params) {
    const response = await axios.get("https://localhost:7233/api/Server", {
      params,
    });
    context.commit("setServers", response.data);
    context.commit(
      "setTotalServers",
      parseInt(response.headers["x-total-count"])
    );
  },
};
