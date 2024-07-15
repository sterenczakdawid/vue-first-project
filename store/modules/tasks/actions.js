export default {
  addTask(context, task) {
    const taskData = {
      id: context.getters.nextId,
      name: task.name,
      created: task.created,
      edited: task.edited,
      serverId: task.serverId,
      appId: task.appId,
    };
    context.commit("addTask", taskData);
  },
};
