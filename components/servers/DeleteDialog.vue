<template>
  <v-dialog v-model="dialog" max-width="500px" persistent>
    <v-card>
      <v-card-title class="font-weight-regular"
        >Are you sure you want to delete {{ itemName }}?</v-card-title
      >
      <!-- <v-card-subtitle v-if="hasSubitems" class="red--text mt-3"
        ><span class="font-weight-bold">Warning!</span> This item has other
        items pinned to it. By deleting this item its subitems will also be
        deleted</v-card-subtitle
      > -->
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
  props: {
    dialog: {
      type: Boolean,
      required: true,
    },
    itemName: {
      type: String,
      required: true,
    },
    // item: {
    //   type: Object,
    // },
  },
  computed: {
    // hasSubitems() {
    //   return (
    //     (Object.hasOwn(this.item, "tasksIds") && this.item.tasksIds.length) ||
    //     Object.hasOwn(this.item, "appsIds" && this.item.appsIds.length)
    //   );
    // },
  },
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
