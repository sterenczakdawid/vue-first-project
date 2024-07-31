import axios from "axios";

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
  updateTask(context, taskData) {
    const { index, item } = taskData;
    context.commit("updateTask", { index, item });
  },
  removeTask(context, taskId) {
    context.commit("removeTask", parseInt(taskId));
  },
  removeServerTasks(context, serverId) {
    context.commit("removeServerTasks", parseInt(serverId));
  },
  detachTasksFromApp(context, appId) {
    context.commit("detachTasksFromApp", parseInt(appId));
  },
  attachTasksToApp(context, app) {
    context.commit("attachTasksToApp", app);
  },
  async loadTasks(context) {
    const response = await axios.get("https://localhost:7233/api/Task");
    // console.log(response.data);
    context.commit("setTasks", response.data);
  },
};
