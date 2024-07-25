<template>
  <v-form class="d-flex flex-column px-3" ref="form" lazy-validation>
    <v-text-field
      v-model.trim="formData.name"
      :rules="nameRules"
      label="Name"
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
      v-model="formData.appId"
      :items="filteredApps"
      label="Application"
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
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    filteredApps() {
      return this.apps.filter((app) => app.serverId == this.formData.serverId);
    },
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    noDataText() {
      return this.formData.serverId === -1
        ? "Select a server to choose from its applications"
        : "Selected server has no applications attached";
    },
  },
  methods: {
    createFormData() {
      return { ...this.initialData };
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
    resetApp() {
      this.formData.appId = 0;
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
        this.resetApp();
      },
      immediate: true,
    },
  },
  mounted() {
    this.formData = this.createFormData();
  },
};
</script>
