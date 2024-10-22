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
    try {
      const res = await axios.post("https://localhost:7233/api/Task", taskData);
      const newTaskId = res.data[res.data.length - 1].id;
      context.commit("addTask", { ...taskData, id: newTaskId });
      await context.dispatch("loadTasks");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async updateTask(context, taskData) {
    const { index, item } = taskData;
    try {
      await axios.put("https://localhost:7233/api/Task", item);
      context.commit("updateTask", { index, item });
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async removeTask(context, taskId) {
    try {
      await axios.delete("https://localhost:7233/api/Task", {
        params: { id: parseInt(taskId) },
      });
      context.commit("removeTask", parseInt(taskId));
      await context.dispatch("loadTasks");
    } catch (error) {
      throw new Error(error.response.data);
    }
  },
  async exportToExcel({ commit }, params) {
    try {
      const response = await axios.get(
        "https://localhost:7233/api/Task/export",
        {
          params,
          responseType: "blob", // Ensure the response is a blob
        }
      );

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", "Tasks.xlsx"); // or whatever you want the file name to be
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } catch (error) {
      alert("Failed to download Excel file", error);
    }
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
  async loadTasks(context, params) {
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
