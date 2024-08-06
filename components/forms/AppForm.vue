<template>
  <v-form class="d-flex flex-column px-3" ref="form" lazy-validation>
    <v-text-field
      :label="$t('headers.name')"
      v-model.trim="formData.name"
      :rules="nameRules"
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
      v-model="formData.tasksIds"
      :items="filteredTasks"
      :label="$t('tasks')"
      multiple
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
      serversRules: [(v) => v > 0 || this.$t("errors.appAttachedToServer")],
    };
  },
  computed: {
    servers() {
      console.log("wywoluje getter serverow do store z appform.vue");
      return this.$store.getters["modules/servers/servers"];
    },
    apps() {
      console.log("wywoluje getter app do store z appform.vue");
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      console.log("wywoluje getter taskow do store z appform.vue");
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
        ? this.$t("noDataText.noServer")
        : this.$t("noDataText.noServerTasks");
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
    uniqueNameRule(v) {
      if (
        this.apps.some((app) => app.name === v && app.id !== this.formData.id)
      ) {
        return this.$t("errors.appNameAlreadyExists");
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
