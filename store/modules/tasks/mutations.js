export default {
  addTask(state, task) {
    state.tasks.push(task);
  },
  removeTask(state, taskId) {
    const taskIndex = state.tasks.findIndex((task) => task.id === taskId);
    state.tasks.splice(taskIndex, 1);
  },
};
