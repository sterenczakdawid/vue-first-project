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
    <!-- <v-menu>
      <template v-slot:activator="{ on }">
        <v-text-field
          :value="created"
          v-on="on"
          label="Created"
          prepend-icon="mdi-calendar"
          :rules="[(v) => !!v || 'Pole wymagane']"
        ></v-text-field>
      </template>
      <v-date-picker v-model="created"></v-date-picker>
    </v-menu> -->
    <!-- <v-menu>
      <template v-slot:activator="{ on }">
        <v-text-field
          :value="edited"
          v-on="on"
          label="Edited"
          prepend-icon="mdi-calendar"
          :rules="[(v) => !!v || 'Pole wymagane']"
        ></v-text-field>
      </template>
      <v-date-picker v-model="edited"></v-date-picker>
    </v-menu> -->
    <!-- <v-btn type="button" class="mx-0 mt-3" @click="cancel" width="100px"
      >Cancel</v-btn
    > -->
    <v-btn class="success mx-0 mt-3" @click="submit" width="100px">Add</v-btn>
  </v-form>
</template>

<script>
export default {
  emits: ["save-data", "update-dialog"],
  data() {
    return {
      name: "",
      // created: null,
      // edited: null,
      appsIds: [],
      tasksIds: [],
      inputRules: [(v) => v.length >= 3 || "Minimalna długość to 3 znaki"],
    };
  },
  methods: {
    submit() {
      if (this.$refs.form.validate()) {
        const formData = {
          name: this.name,
          created: new Date().toISOString(),
          edited: new Date().toISOString(),
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
