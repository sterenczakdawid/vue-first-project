<template>
  <div>
    <div class="d-flex justify-space-between align-center py-8">
      <div>
        <h2>{{ item.name }} - {{ $t("details") }}:</h2>
        <p class="grey--text text--lighten-1 ma-0">
          {{ $t("headers.created") }}: {{ item.created }}
        </p>
        <p class="grey--text text--lighten-1">
          {{ $t("headers.edited") }}: {{ item.edited }}
        </p>
      </div>
      <div class="d-flex flex-column">
        <v-btn class="mb-2" width="125px" @click="editItem">{{
          $t("buttons.edit")
        }}</v-btn>
        <v-btn class="mt-2" color="error" width="125px" @click="deleteItem">{{
          $t("buttons.delete")
        }}</v-btn>
      </div>
    </div>
  </div>
</template>

<script>
import BackButton from "~/components/ui/BackButton.vue";
export default {
  components: {
    BackButton,
  },
  emits: ["editItem", "deleteItem"],
  props: ["item", "itemType"],
  methods: {
    editItem() {
      this.$emit("editItem");
    },
    deleteItem() {
      this.$emit("deleteItem");
    },
  },
};
</script>

<style></style>
