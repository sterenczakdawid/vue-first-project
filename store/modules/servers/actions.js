export default {
  addServer(context, server) {
    const serverData = {
      id: context.getters.nextId,
      name: server.name,
      created: server.created,
      edited: server.edited,
      tasksIds: server.tasksIds,
      appsIds: server.appsIds,
    };
    context.commit("addServer", serverData);
  },
  removeServer(context, serverId) {
    console.log("removing...", serverId);
    context.commit("removeServer", serverId);
  },
  updateServer(context, serverData) {
    const { index, item } = serverData;
    console.log("Przekazuje ", { index, item });
    context.commit("updateServer", { index, item });
  },
};
