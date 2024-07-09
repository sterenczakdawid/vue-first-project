<template>
  <div>
    <h1>{{ id }}</h1>
    <p>{{ name }}</p>
    <!-- <NuxtLink to="./">Powrót</NuxtLink> -->
    <v-data-table
      :headers="headers"
      :items="filteredTasks"
      :items-per-page="5"
      class="row-pointer"
    ></v-data-table>
    <v-data-table
      :headers="headers"
      :items="filteredApps"
      :items-per-page="5"
      class="row-pointer"
    ></v-data-table>
    <v-btn @click="to">Powrót</v-btn>
  </div>
</template>

<script>
export default {
  data() {
    return {
      selectedServer: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    name() {
      return this.selectedServer.name;
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    filteredTasks() {
      if (!this.selectedServer) return [];
      return this.tasks.filter((task) =>
        this.selectedServer.tasksIds.includes(task.id)
      );
    },
    // hasServers() {
    //   return this.$store.getters["modules/servers/hasServers"];
    // },
    headers() {
      return this.$store.getters["modules/tasks/headers"];
    },
    filteredApps() {
      if (!this.selectedServer) return [];
      return this.$store.getters["modules/apps/apps"].filter((app) =>
        this.selectedServer.appsIds.includes(app.id)
      );
    },
  },
  methods: {
    to() {
      this.$router.back();
    },
  },
  created() {
    this.selectedServer = this.$store.getters["modules/servers/servers"].find(
      (server) => server.id == this.id
    );
  },
};
</script>

<style></style>
