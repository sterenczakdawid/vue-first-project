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
  </div>
</template>

<script>
export default {
  computed: {
    servers() {
      return this.$store.getters["servers/servers"];
    },
    hasServers() {
      return this.$store.getters["servers/hasServers"];
    },
  },
};
</script>

<style lang="scss" scoped></style>
