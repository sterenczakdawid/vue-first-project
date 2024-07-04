<template>
  <div>
    <div>Tasks</div>
    <ul v-if="hasServers">
      <li v-for="server in servers" :key="server.id">
        {{ server.name }}
      </li>
    </ul>
    <h2 v-else>nie ma</h2>
    <button>
      <NuxtLink to="tasks/add">Add new task</NuxtLink>
    </button>
    <v-data-table
      :headers="headers"
      :items="servers"
      :items-per-page="5"
      class="elevation-24"
      :loading="loading"
    ></v-data-table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      loading: false,
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    hasServers() {
      return this.$store.getters["modules/servers/hasServers"];
    },
    headers() {
      return this.$store.getters["modules/servers/headers"];
    },
  },
};
</script>

<style lang="scss" scoped></style>
