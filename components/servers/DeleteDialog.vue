<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >Are you sure you want to delete {{ itemName }}?</v-card-title
      >
      <v-card-subtitle v-if="itemType === 'app'" class="red--text mt-1"
        ><span class="font-weight-bold">Warning!</span> Deleting this
        application will detach all associated tasks, leaving them unlinked to
        any application!
      </v-card-subtitle>
      <v-card-subtitle v-else-if="itemType === 'server'" class="red--text mt-1"
        ><span class="font-weight-bold">Warning!</span> Deleting this server
        will automatically remove all associated tasks and
        applications!</v-card-subtitle
      >
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn elevation="4" @click="cancel" width="120px">Cancel</v-btn>
        <v-btn elevation="4" @click="confirm" color="error" width="120px"
          >delete</v-btn
        >
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
