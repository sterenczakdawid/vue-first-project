export default {
  tasks(state) {
    return state.tasks;
  },
  totalTasks(state) {
    return state.totalTasks;
  },
  totalPages() {
    return Math.ceil(this.totalTasks / this.pageSize);
  },
  hasTasks(state) {
    return state.tasks && state.tasks.length > 0;
  },
  nextId: (state) => {
    const ids = state.tasks.map((task) => parseInt(task.id, 10));
    return ids.length ? Math.max(...ids) + 1 : 1;
  },
};
