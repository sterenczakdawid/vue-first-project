<template>
  <v-dialog max-width="700px" v-model="isOpen" persistent>
    <!-- <template v-slot:activator="{ on }">
      <v-btn text v-on="on">Dodaj</v-btn>
    </template> -->
    <v-card>
      <v-card-title>
        <h2>Dodawanie</h2>
      </v-card-title>
      <v-card-text>
        <add-server-form @save-data="saveData" @update-dialog="closeDialog" />
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text @click="closeDialog">Cancel</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import AddServerForm from "~/components/servers/AddServerForm.vue";
export default {
  components: { AddServerForm },
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
    // saveData(formData) {
    //   this.$emit("save-data", formData);
    //   // this.$store.dispatch("modules/servers/addServer", formData);
    // },
    // updateDialog(value) {
    //   this.$emit("update-dialog", false);
    //   // this.dialog = value;
    // },
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
