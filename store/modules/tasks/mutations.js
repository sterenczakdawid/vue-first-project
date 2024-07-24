export default {
  addTask(state, task) {
    state.tasks.push(task);
  },
  updateTask(state, { index, item }) {
    state.tasks.splice(index, 1, item);
  },
  removeTask(state, taskId) {
    const taskIndex = state.tasks.findIndex((task) => task.id === taskId);
    state.tasks.splice(taskIndex, 1);
  },
};
