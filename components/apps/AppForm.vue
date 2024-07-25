<template>
  <v-form class="d-flex flex-column px-3" ref="form" lazy-validation>
    <v-text-field
      label="Name"
      v-model.trim="formData.name"
      :rules="nameRules"
      required
    ></v-text-field>
    <v-select
      v-model="formData.serverId"
      :items="servers"
      :rules="serversRules"
      label="Server"
      item-text="name"
      item-value="id"
      required
    ></v-select>
    <v-select
      v-model="formData.tasksIds"
      :items="filteredTasks"
      label="Tasks"
      multiple
      item-text="name"
      item-value="id"
      :no-data-text="noDataText"
      clearable
    ></v-select>
    <div class="align-self-end">
      <v-btn @click="close"> Cancel </v-btn>
      <v-btn class="success" @click="submit">Submit</v-btn>
    </div>
  </v-form>
</template>

<script>
export default {
  emits: ["submit", "close"],
  props: ["initialData"],
  data() {
    return {
      formData: this.createFormData(),
      nameRules: [
        (v) => !!v.trim() || "Name is required",
        (v) => v.trim().length >= 3 || "Name must have at least 3 characters",
      ],
      serversRules: [(v) => v > 0 || "Task has to be attached to a server"],
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    tasksIds() {
      return this.tasks
        .filter((task) => task.appId === this.initialData.id)
        .map((task) => task.id);
    },
    filteredTasks() {
      return this.tasks.filter((task) => {
        return (
          task.serverId === this.formData.serverId &&
          (task.appId === this.formData.id ||
            task.appId === 0 ||
            task.appId == null)
        );
      });
    },
    noDataText() {
      return this.formData.serverId === -1
        ? "Select a server to choose from its tasks"
        : "Selected server has no tasks attached";
    },
  },
  methods: {
    createFormData() {
      return { ...this.initialData, tasksIds: this.tasksIds };
    },
    close() {
      this.$refs.form.resetValidation();
      this.$emit("close");
    },
    submit() {
      if (this.$refs.form.validate()) {
        this.$emit("submit", this.formData);
        this.formData = this.createFormData();
        this.$refs.form.resetValidation();
      }
    },
    resetTasks() {
      this.formData.tasksIds = [];
    },
  },
  watch: {
    initialData: {
      handler() {
        this.formData = this.createFormData();
      },
      deep: true,
    },
    "formData.serverId": {
      handler() {
        this.resetTasks();
      },
      immediate: true,
    },
  },
  mounted() {
    this.formData = this.createFormData();
  },
};
</script>
