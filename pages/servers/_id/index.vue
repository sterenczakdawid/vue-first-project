<template>
  <div>
    <!-- <h1>{{ id }}</h1> -->
    <h2 class="pt-8 pb-4">{{ name }} - details</h2>
    <p class="grey--text text--lighten-1 ma-0">Created: {{ created }}</p>
    <p class="grey--text text--lighten-1">Edited: {{ edited }}</p>
    <v-tabs v-model="tab" centered grow>
      <v-tab>Applications</v-tab>
      <v-tab>Tasks</v-tab>
    </v-tabs>
    <v-tabs-items v-model="tab">
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredApps"
          :items-per-page="5"
          class="row-pointer"
          @click:row="handleClickApp"
        ></v-data-table
      ></v-tab-item>
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredTasks"
          :items-per-page="5"
          class="row-pointer"
          @click:row="handleClick"
        ></v-data-table
      ></v-tab-item>
    </v-tabs-items>

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
      tab: null,
      selectedServer: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    name() {
      return this.selectedServer.name;
    },
    created() {
      return this.selectedServer.created;
    },
    edited() {
      return this.selectedServer.edited;
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
      return this.$store.getters.getHeaders;
    },
    filteredApps() {
      if (!this.selectedServer) return [];
      return this.$store.getters["modules/apps/apps"].filter((app) =>
        this.selectedServer.appsIds.includes(app.id)
      );
    },
  },
  methods: {
    handleClick(item) {
      console.log(item.id);
      this.$router.push("/tasks/" + item.id);
    },
    handleClickApp(item) {
      this.$router.push("/apps/" + item.id);
    },
  },
  created() {
    this.selectedServer = this.$store.getters["modules/servers/servers"].find(
      (server) => server.id == this.id
    );
  },
};
</script>

<style>
.row-pointer >>> tbody tr :hover {
  cursor: pointer;
}
</style>
