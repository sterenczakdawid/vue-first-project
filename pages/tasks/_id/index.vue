<template>
  <div>
    <h1>{{ id }}</h1>
    <p>{{ name }}</p>
    <NuxtLink to="./">Powrót</NuxtLink>
    <!-- <v-btn @click="to">Powrót</v-btn> -->
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
