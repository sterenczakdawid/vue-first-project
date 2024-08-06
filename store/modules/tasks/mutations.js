export default {
  setTasks(state, payload) {
    state.tasks = payload;
  },
  setTotalTasks(state, payload) {
    state.totalTasks = payload;
  },
  addTask(state, task) {
    console.log("tasks push: ", task);
    state.tasks.push(task);
  },
  updateTask(state, { index, item }) {
    state.tasks.splice(index, 1, item);
  },
  removeTask(state, taskId) {
    const taskIndex = state.tasks.findIndex((task) => task.id === taskId);
    state.tasks.splice(taskIndex, 1);
  },
  removeServerTasks(state, serverId) {
    state.tasks = state.tasks.filter((task) => task.serverId != serverId);
  },
  detachTasksFromApp(state, appId) {
    state.tasks.forEach((task) => {
      if (task.appId === appId) {
        task.appId = 0;
      }
    });
  },
  attachTasksToApp(state, { id, tasksIds }) {
    if (tasksIds) {
      state.tasks.forEach((task) => {
        if (tasksIds.includes(task.id)) {
          task.appId = id;
        }
      });
    }
  },
};
