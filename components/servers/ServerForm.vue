<template>
  <v-form class="px-3" ref="form">
    <v-text-field
      label="Name"
      v-model.trim="formData.name"
      :rules="inputRules"
    ></v-text-field>
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
    <v-btn class="success mx-0 mt-3" @click="submit" width="100px">{{
      submitLabel
    }}</v-btn>
  </v-form>
</template>

<script>
export default {
  props: {
    initialData: {
      type: Object,
      default: () => ({
        name: "",
        appsIds: [],
        tasksIds: [],
      }),
    },
    submitLabel: {
      type: String,
      default: "Submit",
    },
  },
  data() {
    return {
      formData: { ...this.initialData },
      inputRules: [(v) => v.length >= 3 || "Minimalna długość to 3 znaki"],
      options: {
        day: "numeric",
        month: "numeric",
        year: "numeric",
        hour: "numeric",
        minute: "numeric",
        second: "numeric",
      },
    };
  },
  methods: {
    submit() {
      if (this.$refs.form.validate()) {
        const data = {
          ...this.formData,
          edited: new Date().toLocaleString("en-GB", this.options),
        };
        if (!this.formData.created) {
          data.created = new Date().toLocaleString("en-GB", this.options);
        }
        this.$emit("submit", data);
        this.resetForm();
      }
    },
    cancel() {
      this.$emit("update-dialog", false);
    },
    resetForm() {
      this.formData = { ...this.initialData };
    },
  },
  computed: {
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
  },
};
</script>
