<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="apps"
      :items-per-page="5"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Apps</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="openDialog('add')">add new app</v-btn>
          <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="'app'">
            <app-form
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            ></app-form>
          </form-dialog>
          <delete-dialog
            :dialog.sync="dialogDelete"
            :itemName="editedItemName"
            @confirm-delete="deleteItemConfirm"
            @cancel-delete="closeDelete"
          />
        </v-toolbar>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small class="pa-2 mr-2" @click.stop="editItem(item)">
          mdi-pencil
        </v-icon>
        <v-icon small class="pa-2" @click.stop="deleteItem(item)">
          mdi-delete
        </v-icon>
      </template>
    </v-data-table>
  </div>
</template>

<script>
import DataTable from "~/components/DataTable.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import FormDialog from "~/components/FormDialog.vue";
import TaskForm from "~/components/tasks/TaskForm.vue";
import AppForm from "~/components/apps/AppForm.vue";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    DataTable,
    AppForm,
  },
  data() {
    return {
      dialog: false,
      mode: "add",
      dialogDelete: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: -1,
        tasksIds: [],
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: -1,
      },
    };
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
    },
    deleteItemConfirm(index) {
      this.$store.dispatch("modules/apps/removeApp", index);
    },
    editItem(item) {
      this.mode = "edit";
      this.editedIndex = this.apps.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/apps/removeApp", this.editedIndex);
      this.closeDelete();
    },
    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    openDialog(mode) {
      this.mode = mode;
      this.dialog = true;
    },
    submit(formData) {
      // console.log("submit w apps/index.vue");
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      console.log("data: ", data);
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/apps/updateApp", {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch("modules/apps/addApp", data);
      }
      this.dialog = false;
    },
  },
  computed: {
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    headers() {
      return this.$store.getters.getHeaders;
    },
    editedItemName() {
      return this.editedItem.name;
    },
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
</style>
