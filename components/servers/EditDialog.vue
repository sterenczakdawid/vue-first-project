<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular">Edit</v-card-title>
      <v-card-text>
        <server-form
          :initialData="editedItem"
          :submitLabel="'Save'"
          @submit="save"
        />
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text @click="close"> Cancel </v-btn>
        <!-- <v-btn color="blue darken-1" text @click="save"> Save </v-btn> -->
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import ServerForm from "~/components/servers/ServerForm.vue";
export default {
  components: { ServerForm },
  props: {
    dialog: {
      type: Boolean,
      required: true,
    },
    editedItem: {
      type: Object,
      required: true,
    },
  },
  methods: {
    close() {
      this.$emit("close");
    },
    save(data) {
      this.$emit("save", data);
    },
  },
};
</script>

<style></style>
