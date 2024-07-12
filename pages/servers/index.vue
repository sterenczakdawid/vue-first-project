<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="servers"
      :items-per-page="5"
      :loading="loading"
      v-if="hasServers"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Servers</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <add-server-dialog v-model="dialog" @save-data="saveData" />
          <edit-dialog
            :dialog.sync="dialogEdit"
            :editedItem="editedItem"
            @close="close"
            @save="save"
          />
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
    <!-- <h2 v-else>Nie ma danych</h2> -->
  </div>
</template>

<script>
import AddServerDialog from "~/components/servers/AddServerDialog.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import EditDialog from "~/components/servers/EditDialog.vue";
export default {
  components: {
    AddServerDialog,
    DeleteDialog,
    EditDialog,
  },
  data() {
    return {
      dialog: false,
      loading: false,
      dialogDelete: false,
      dialogEdit: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
    };
  },
  watch: {
    deleteDialog(value) {
      value || this.closeDelete();
    },
  },
  methods: {
    handleClick(item) {
      // console.log(item.id);
      this.$router.push(this.$route.path + "/" + item.id);
    },
    saveData(formData) {
      this.$store.dispatch("modules/servers/addServer", formData);
      this.dialog = false;
    },
    updateDialog(value) {
      this.dialog = value;
    },
    editItem(item) {
      console.log("editing " + item.name, item.id);
      this.editedIndex = this.servers.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.editedItem.edited = new Date();
      this.dialogEdit = true;
    },
    deleteItem(item) {
      // this.editedIndex = this.servers.indexOf(item);
      this.editedIndex = item.id;
      // console.log(this.editedIndex);
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      // console.log("actually removing ", this.editedIndex);
      this.$store.dispatch("modules/servers/removeServer", this.editedIndex);
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
      // this.editedItem = Object.assign({}, this.defaultItem);
      // this.editedIndex = -1;
      this.dialogEdit = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    // save() {
    //   if (this.editedIndex > -1) {
    //     // Object.assign(this.servers[this.editedIndex], this.editedItem);
    //     console.log("edited: ", this.editedItem);
    //     this.$store.dispatch("modules/servers/updateServer", {
    //       index: this.editedIndex,
    //       item: this.editedItem,
    //     });
    //   }
    //   this.dialogEdit = false;
    //   // else {
    //   //   this.servers.push(this.editedItem);
    //   // }
    //   // this.close();
    // },
    save(data) {
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/servers/updateServer", {
          index: this.editedIndex,
          item: data,
        });
      }
      this.dialogEdit = false;
    },
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    hasServers() {
      return this.$store.getters["modules/servers/hasServers"];
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
