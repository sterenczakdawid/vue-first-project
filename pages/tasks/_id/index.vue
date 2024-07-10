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
      selectedTask: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    name() {
      return this.selectedTask.name;
    },
  },
  created() {
    this.selectedTask = this.$store.getters["modules/tasks/tasks"].find(
      (task) => task.id == this.id
    );
  },
};
</script>

<style></style>
