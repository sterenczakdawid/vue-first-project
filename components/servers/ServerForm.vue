<template>
  <v-form class="d-flex flex-column px-3" ref="form">
    <v-text-field
      label="Name"
      v-model.trim="formData.name"
      :rules="nameRules"
      required
    ></v-text-field>
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
        (v) => !!v || "Name is required",
        (v) => v.length >= 3 || "Name must have at least 3 characters",
      ],
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
