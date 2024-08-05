import axios from "axios";

export default {
  async addTask(context, task) {
    const taskData = {
      name: task.name,
      created: task.created,
      edited: task.edited,
      serverId: task.serverId,
      appId: task.appId,
    };
    const res = await axios.post("https://localhost:7233/api/Task", taskData);
    const newTaskId = res.data[res.data.length - 1].id;
    context.commit("addTask", { ...taskData, id: newTaskId });
  },
  async updateTask(context, taskData) {
    const { index, item } = taskData;
    console.log(item);
    const res = await axios.put("https://localhost:7233/api/Task", item);
    // await context.dispatch("loadTasks");
    context.commit("updateTask", { index, item });
  },
  async removeTask(context, taskId) {
    const res = await axios.delete("https://localhost:7233/api/Task", {
      params: { id: parseInt(taskId) },
    });
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
  // async loadTasks(context) {
  //   const response = await axios.get("https://localhost:7233/api/Task");
  //   console.log("tasks loaded", response);
  //   context.commit("setTasks", response.data);
  // },
  // async loadTasks(context, { page = 1, pageSize = 10 } = {}) {
  //   // console.log(`actions parameters: page: ${page}, pageSize: ${pageSize}`);
  //   const response = await axios.get("https://localhost:7233/api/Task", {
  //     params: { page, pageSize },
  //   });
  //   // console.log("actions response: ", response.data);
  //   context.commit("setTasks", response.data);
  //   context.commit(
  //     "setTotalTasks",
  //     parseInt(response.headers["x-total-count"])
  //   );
  //   // console.log("Pobrano taski: ", response);
  //   // console.log("Total: ", parseInt(response.headers["x-total-count"]));
  // },
  async loadTasks(context, params) {
    // console.log("load tasks w store wywolane");
    const response = await axios.get("https://localhost:7233/api/Task", {
      params,
    });
    context.commit("setTasks", response.data);
    context.commit(
      "setTotalTasks",
      parseInt(response.headers["x-total-count"])
    );
  },
};
