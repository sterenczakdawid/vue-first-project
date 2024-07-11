<template>
  <v-form class="px-3" ref="form">
    <v-text-field
      label="Name"
      v-model.trim="name"
      :rules="inputRules"
    ></v-text-field>
    <v-select
      v-model="appsIds"
      :items="apps"
      label="Add applications to this server"
      multiple
      item-text="name"
      item-value="id"
    ></v-select>
    <v-select
      v-model="tasksIds"
      :items="tasks"
      label="Add tasks to this server"
      multiple
      item-text="name"
      item-value="id"
    ></v-select>
    <v-btn class="success mx-0 mt-3" @click="submit" width="100px">Add</v-btn>
  </v-form>
</template>

<script>
export default {
  emits: ["save-data", "update-dialog"],
  data() {
    return {
      name: "",
      appsIds: [],
      tasksIds: [],
      inputRules: [(v) => v.length >= 3 || "Minimalna długość to 3 znaki"],
      options: {
        // weekday: "short",
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
        const formData = {
          name: this.name,
          created: new Date().toLocaleString("en-US", this.options),
          // edited: new Date().toLocaleString("en-GB", this.options),
          edited: new Date().toISOString().split("T")[0],
          tasksIds: this.tasksIds,
          appsIds: this.appsIds,
        };

        // console.log(formData);
        this.$emit("save-data", formData);
        this.$emit("update-dialog", false);

        this.name = "";
        this.created = null;
        this.edited = null;
      }
    },
    cancel() {
      this.$emit("update-dialog", false);
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
