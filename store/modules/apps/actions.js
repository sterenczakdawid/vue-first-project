export default {
  addApp(context, app) {
    const appData = {
      id: context.getters.nextId,
      name: app.name,
      created: app.created,
      edited: app.edited,
      serverId: app.serverId,
    };
    context.commit("addApp", appData);
    const tasks = context.rootGetters["modules/tasks/tasks"];
    for (const taskId of app.tasksIds) {
      let taskData = tasks.find((task) => task.id == taskId);
      taskData = { ...taskData, appId: appData.id };
      const taskIndex = tasks.findIndex((task) => task.id == taskId);
      console.log("przekazuje do update'u taska: ", taskIndex, taskData);
      context.dispatch(
        "modules/tasks/updateTask",
        {
          index: taskIndex,
          item: taskData,
        },
        { root: true }
      );
    }
  },
  updateApp(context, appData) {
    const { index, item } = appData;
    console.log("update app w store, otrzymalem: ", appData);
    context.commit("updateApp", { index, item });
    const tasks = context.rootGetters["modules/tasks/tasks"];
    for (const task of tasks) {
      if (task.appId == item.id) {
        const taskIndex = tasks.findIndex((taskk) => taskk.id == task.id);
        const taskData = { ...task, appId: -1 };
        context.dispatch(
          "modules/tasks/updateTask",
          {
            index: taskIndex,
            item: taskData,
          },
          { root: true }
        );
      }
    }
    if (item.tasksIds) {
      for (const taskId of item.tasksIds) {
        let taskData = tasks.find((task) => task.id == taskId);
        taskData = { ...taskData, appId: item.id };
        const taskIndex = tasks.findIndex((task) => task.id == taskId);
        console.log("przekazuje do update'u taska: ", taskIndex, taskData);
        context.dispatch(
          "modules/tasks/updateTask",
          {
            index: taskIndex,
            item: taskData,
          },
          { root: true }
        );
      }
    }
  },
  removeApp(context, appId) {
    context.commit("removeApp", parseInt(appId));
  },
};
