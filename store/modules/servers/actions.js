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
  removeServer(context, server) {
    console.log("removing...");
    context.commit("removeServer", server);
  },
};
