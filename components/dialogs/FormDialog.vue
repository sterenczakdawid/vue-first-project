<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >{{ dialogTitle }}{{ itemTypeTranslated }}</v-card-title
      >
      <v-card-text>
        <v-container v-if="dialog">
          <slot></slot>
        </v-container>
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
      return this.mode === "add" ? this.$t("addNew") : this.$t("buttons.edit");
    },
    itemTypeTranslated() {
      if (this.itemType === "server") {
        return this.$t("server").toLowerCase();
      } else if (this.itemType === "app") {
        return this.$t("appAdd").toLowerCase();
      } else return this.$t("task").toLowerCase();
    },
  },
};
</script>
