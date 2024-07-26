<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >{{ $t("deleteConfirm") }} {{ itemName }}?</v-card-title
      >
      <v-card-subtitle v-if="itemType === 'app'" class="red--text mt-1"
        ><span class="font-weight-bold">{{ $t("warning") }}</span>
        {{ $t("deletingApp") }}
      </v-card-subtitle>
      <v-card-subtitle v-else-if="itemType === 'server'" class="red--text mt-1"
        ><span class="font-weight-bold">{{ $t("warning") }}</span>
        {{ $t("deletingServer") }}</v-card-subtitle
      >
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn elevation="4" @click="cancel" width="120px">{{
          $t("buttons.cancel")
        }}</v-btn>
        <v-btn elevation="4" @click="confirm" color="error" width="120px">{{
          $t("buttons.delete")
        }}</v-btn>
        <v-spacer></v-spacer>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  props: ["dialog", "itemName", "itemType"],
  methods: {
    cancel() {
      this.$emit("update:dialog", false);
      this.$emit("cancel-delete");
    },
    confirm() {
      this.$emit("confirm-delete");
    },
  },
};
</script>

<style></style>
