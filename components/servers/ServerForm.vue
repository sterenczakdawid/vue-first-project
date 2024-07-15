<template>
  <v-form class="px-3" ref="form">
    <v-text-field label="Name" v-model.trim="formData.name"></v-text-field>
    <v-select
      v-model="formData.appsIds"
      :items="apps"
      label="Applications"
      multiple
      item-text="name"
      item-value="id"
    ></v-select>
    <v-select
      v-model="formData.tasksIds"
      :items="tasks"
      label="Tasks"
      multiple
      item-text="name"
      item-value="id"
    ></v-select>
  </v-form>
</template>

<script>
export default {
  name: "ServerForm",
  props: {
    initialData: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      formData: { ...this.initialData },
    };
  },
  computed: {
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
  },
  methods: {
    validate() {
      return this.$refs.form.validate();
    },
  },
  watch: {
    initialData: {
      handler(newData) {
        this.formData = { ...newData };
      },
      deep: true,
    },
  },
};
</script>

<style scoped></style>
