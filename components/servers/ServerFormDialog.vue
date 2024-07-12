<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular">{{ dialogTitle }}</v-card-title>
      <v-card-text>
        <v-form class="px-3" ref="form">
          <v-text-field
            label="Name"
            v-model.trim="editedItem.name"
          ></v-text-field>
          <v-select
            v-model="editedItem.appsIds"
            :items="apps"
            label="Applications"
            multiple
            item-text="name"
            item-value="id"
          ></v-select>
          <v-select
            v-model="editedItem.tasksIds"
            :items="tasks"
            label="Tasks"
            multiple
            item-text="name"
            item-value="id"
          ></v-select>
        </v-form>
        <!-- <server-form
          :initialData="editedItem"
          :submitLabel="'Save'"
          @submit="save"
        /> -->
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn @click="close"> Cancel </v-btn>
        <v-btn class="success" @click="submit">Submit</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import ServerForm from "~/components/servers/old/ServerForm.vue";
export default {
  components: { ServerForm },
  emits: ["submit", "close"],
  props: {
    dialog: { type: Boolean, required: true },
    mode: { type: String, required: true },
    editedItem: { type: Object, required: true },
  },
  methods: {
    close() {
      this.$emit("close");
    },
    submit() {
      if (this.$refs.form.validate()) {
        const data = {
          ...this.editedItem,
          edited: new Date().toLocaleString(),
        };
        if (!this.editedItem.created) {
          data.created = new Date().toLocaleString();
        }
        this.$emit("submit", data);
      }
    },
  },
  computed: {
    dialogTitle() {
      return this.mode === "add" ? "Add new item" : "Edit item";
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
  },
};
</script>

<style></style>
