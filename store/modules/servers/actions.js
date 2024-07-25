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
};
