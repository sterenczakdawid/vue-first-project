<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >{{ dialogTitle }}{{ itemType }}</v-card-title
      >
      <v-card-text>
        <slot></slot>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: {
    dialog: { type: Boolean, required: true },
    mode: { type: String, required: true },
    itemType: { type: String, required: true },
  },
  computed: {
    dialogTitle() {
      return this.mode === "add" ? "Add new " : "Edit ";
    },
  },
};
</script>
