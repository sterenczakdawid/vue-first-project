<template>
  <v-form class="d-flex flex-column px-3" ref="form">
    <v-text-field
      :label="$t('headers.name')"
      v-model.trim="formData.name"
      :rules="nameRules"
      required
    ></v-text-field>
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
        (v) => !!v || this.$t("errors.nameRequired"),
        (v) => v.length >= 3 || this.$t("errors.nameTooShort"),
        this.uniqueNameRule,
      ],
    };
  },
  computed: {
    apps() {
      console.log("wywoluje getter app do store z serverform.vue");
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      console.log("wywoluje getter taskow do store z serverform.vue");
      return this.$store.getters["modules/tasks/tasks"];
    },
    servers() {
      console.log("wywoluje getter serverow do store z serverform.vue");
      return this.$store.getters["modules/servers/servers"];
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
    uniqueNameRule(v) {
      if (
        this.servers.some(
          (server) => server.name === v && server.id !== this.formData.id
        )
      ) {
        return this.$t("errors.serverNameAlreadyExists");
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
  },
  mounted() {
    this.formData = this.createFormData();
  },
};
</script>
