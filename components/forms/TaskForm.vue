<template>
  <v-form class="d-flex flex-column px-3" ref="form" lazy-validation>
    <v-text-field
      v-model.trim="formData.name"
      :rules="nameRules"
      :label="$t('headers.name')"
      required
    ></v-text-field>
    <v-select
      v-model="formData.serverId"
      :items="servers"
      :rules="serversRules"
      :label="$t('server')"
      item-text="name"
      item-value="id"
      required
    ></v-select>
    <v-select
      v-model="formData.appId"
      :items="filteredApps"
      :label="$t('app')"
      item-text="name"
      item-value="id"
      :no-data-text="noDataText"
      clearable
    ></v-select>
    <div class="align-self-end">
      <v-btn @click="close"> {{ $t("buttons.cancel") }} </v-btn>
      <v-btn class="success" @click="submit">{{ $t("buttons.submit") }}</v-btn>
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
        (v) => !!v.trim() || this.$t("errors.nameRequired"),
        (v) => v.trim().length >= 3 || this.$t("errors.nameTooShort"),
        this.uniqueNameRule,
      ],
      serversRules: [(v) => v > 0 || this.$t("errors.taskAttachedToServer")],
    };
  },
  computed: {
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    filteredApps() {
      return this.apps.filter((app) => app.serverId == this.formData.serverId);
    },
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    noDataText() {
      return this.formData.serverId === -1
        ? this.$t("noDataText.noServerApp")
        : this.$t("noDataText.noServerApps");
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
    uniqueNameRule(v) {
      if (
        this.tasks.some(
          (task) => task.name === v && task.id !== this.formData.id
        )
      ) {
        return this.$t("errors.taskNameAlreadyExists");
      }
      return true;
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
