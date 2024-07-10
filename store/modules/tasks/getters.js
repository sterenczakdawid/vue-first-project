export default {
  tasks(state) {
    return state.tasks;
  },
  hasTasks(state) {
    return state.tasks && state.tasks.length > 0;
  },
  nextId: (state) => {
    const ids = state.tasks.map((task) => parseInt(task.id, 10));
    return ids.length ? Math.max(...ids) + 1 : 1;
  },
};
