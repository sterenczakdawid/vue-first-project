<template>
  <v-dialog max-width="700px" v-model="isOpen" persistent>
    <template v-slot:activator="{ on }">
      <v-btn color="primary" v-on="on">Add new server</v-btn>
    </template>
    <v-card>
      <v-card-title>
        <h2>Dodawanie</h2>
      </v-card-title>
      <v-card-text>
        <server-form :submitLabel="'Add'" @submit="saveData" />
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text @click="closeDialog">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import ServerForm from "~/components/servers/ServerForm.vue";
export default {
  components: { ServerForm },
  props: {
    value: Boolean,
  },
  computed: {
    isOpen: {
      get() {
        return this.value;
      },
      set(value) {
        this.$emit("input", value);
      },
    },
  },
  methods: {
    saveData(formData) {
      this.$emit("save-data", formData);
      this.isOpen = false;
    },
    closeDialog() {
      this.isOpen = false;
    },
  },
};
</script>

<style></style>
