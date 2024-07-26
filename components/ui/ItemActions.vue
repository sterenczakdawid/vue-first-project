<template>
  <div>
    <v-btn color="primary" @click="openDialog('add')">{{ $t("addApp") }}</v-btn>
    <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="'app'">
      <!-- <app-form :initialData="editedItem"></app-form> -->
      <!-- <app-form
        :initialData="editedItem"
        @close="closeDialog(false)"
        @submit="submit"
      ></app-form> -->
    </form-dialog>
    <!-- <delete-dialog
      :dialog.sync="dialogDelete"
      :itemName="editedItemName"
      :itemType="'app'"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog(true)"
    /> -->
  </div>
</template>

<script>
import FormDialog from "~/components/dialogs/FormDialog.vue";
import AppForm from "~/components/forms/AppForm.vue";
export default {
  components: { FormDialog, AppForm },
  data() {
    return {
      dialog: false,
      mode: "add",
      editedItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: 0,
        tasksIds: [],
      },
    };
  },
  methods: {
    openDialog(mode) {
      this.mode = mode;
      this.editedItem = Object.assign({}, this.defaultItem);
      this.dialog = true;
    },
  },
};
</script>

<style></style>
