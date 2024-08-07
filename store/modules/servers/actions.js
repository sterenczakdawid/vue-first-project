import axios from "axios";

export default {
  async addServer(context, server) {
    const serverData = {
      name: server.name,
      created: server.created,
      edited: server.edited,
    };
    try {
      const res = await axios.post(
        "https://localhost:7233/api/Server",
        serverData
      );
      context.commit("addServer", {
        ...serverData,
        id: res.data[res.data.length - 1].id,
      });
      await context.dispatch("loadServers");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async removeServer(context, serverId) {
    try {
      await axios.delete(`https://localhost:7233/api/Task/server/${serverId}`);
      await axios.delete("https://localhost:7233/api/Server", {
        params: { id: parseInt(serverId) },
      });
      await axios.delete(`https://localhost:7233/api/App/server/${serverId}`);
      context.commit("modules/tasks/removeServerTasks", parseInt(serverId), {
        root: true,
      });
      context.commit("modules/apps/removeServerApps", parseInt(serverId), {
        root: true,
      });
      context.commit("removeServer", parseInt(serverId));
      await context.dispatch("loadServers");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async updateServer(context, serverData) {
    try {
      const { index, item } = serverData;
      await axios.put("https://localhost:7233/api/Server", item);
      context.commit("updateServer", { index, item });
    } catch (error) {
      throw new Error(error.response.data);
    }
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
