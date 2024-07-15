<template>
  <div>
    <h1>{{ id }}</h1>
    <p>{{ name }}</p>
    <p>{{ serverId }}</p>
    <p>{{ appId }}</p>
    <v-btn @click="removeTaskFromServer">Odepnij od serwera</v-btn>
    <back-button />
  </div>
</template>

<script>
import BackButton from "~/components/utils/BackButton.vue";
export default {
  components: {
    BackButton,
  },
  data() {
    return {
      selectedTask: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    name() {
      return this.selectedTask.name;
    },
    serverId() {
      return this.selectedTask.serverId;
    },
    appId() {
      return this.selectedTask.appId;
    },
  },
  created() {
    this.selectedTask = this.$store.getters["modules/tasks/tasks"].find(
      (task) => task.id == this.id
    );
  },
  methods: {
    removeTaskFromServer() {
      const server = this.$store.getters["modules/servers/servers"].find(
        (server) => server.id === this.selectedTask.serverId
      );
      const tasks = server.tasksIds.filter(
        (taskId) => taskId != this.selectedTask.id
      );
      console.log(tasks);
      // console.log(tasks.indexOf(6));
      const newServer = { ...server, taskIds: tasks };
      console.log(newServer);
      // console.log(server.id, server.name);
      this.$store.dispatch("modules/servers/updateServer", {
        index: this.servers.indexOf(server.id),
        item: newServer,
      });
    },
  },
};
</script>

<style></style>
