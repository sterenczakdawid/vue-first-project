<template>
  <div>
    <h1>{{ id }}</h1>
    <p>{{ name }}</p>
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
      selectedApp: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    name() {
      return this.selectedApp.name;
    },
  },
  created() {
    this.selectedApp = this.$store.getters["modules/apps/apps"].find(
      (app) => app.id == this.id
    );
  },
};
</script>

<style></style>
