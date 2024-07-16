<template>
  <div>
    <item-details
      :itemType="'Server'"
      :item="this.selectedServer"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <v-tabs v-model="tab" grow>
      <v-tab>Applications</v-tab>
      <v-tab>Tasks</v-tab>
    </v-tabs>
    <v-tabs-items v-model="tab">
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredApps"
          :items-per-page="5"
          class="row-pointer"
          @click:row="handleClickApp"
        ></v-data-table
      ></v-tab-item>
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredTasks"
          :items-per-page="5"
          class="row-pointer"
          @click:row="handleClick"
        ></v-data-table
      ></v-tab-item>
    </v-tabs-items>
    <delete-dialog
      :dialog.sync="dialogDelete"
      :itemName="name"
      :item="this.selectedServer"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
    <server-form-dialog
      :dialog.sync="dialogEdit"
      :mode="'edit'"
      :itemType="'server'"
      ><ServerForm
        ref="serverForm"
        :initialData="this.selectedServer"
        @close="closeDialog('edit')"
        @submit="submit"
    /></server-form-dialog>
    <back-button />
  </div>
</template>

<script>
import BackButton from "~/components/utils/BackButton.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import ServerFormDialog from "~/components/FormDialog.vue";
import ServerForm from "~/components/servers/ServerForm.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";
export default {
  components: {
    BackButton,
    DeleteDialog,
    ServerFormDialog,
    ServerForm,
    ItemDetails,
  },
  data() {
    return {
      dialogDelete: false,
      dialogEdit: false,
      editedItem: {},
      tab: null,
      selectedServer: null,
      id: this.$route.params.id,
    };
  },
  computed: {
    name() {
      return this.selectedServer.name;
    },
    created() {
      return this.selectedServer.created;
    },
    edited() {
      return this.selectedServer.edited;
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    filteredTasks() {
      if (!this.selectedServer) return [];
      return this.tasks.filter((task) =>
        this.selectedServer.tasksIds.includes(task.id)
      );
    },
    headers() {
      return this.$store.getters.getHeaders;
    },
    filteredApps() {
      if (!this.selectedServer) return [];
      return this.$store.getters["modules/apps/apps"].filter((app) =>
        this.selectedServer.appsIds.includes(app.id)
      );
    },
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
  },
  methods: {
    handleClick(item) {
      console.log(item.id);
      this.$router.push("/tasks/" + item.id);
    },
    handleClickApp(item) {
      this.$router.push("/apps/" + item.id);
    },
    deleteItem() {
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/servers/removeServer", this.id);
      this.closeDialog("del");
      this.$router.replace("/servers");
    },
    editItem() {
      this.editedItem = Object.assign({}, this.selectedServer);
      this.dialogEdit = true;
    },
    editItemSubmit(data) {
      this.$store
        .dispatch("modules/servers/updateServer", {
          index: this.servers.indexOf(this.selectedServer),
          item: data,
        })
        .then(() => {
          this.selectedServer = { ...data };
        });
      this.dialogEdit = false;
    },
    submit2(data) {
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/servers/updateServer", {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch("modules/servers/addServer", data);
      }
      console.log(data);
      this.dialog = false;
    },
    submit({ form, formData }) {
      if (form.validate()) {
        const data = {
          ...formData,
          edited: new Date().toLocaleString(),
        };
        if (!formData.created) {
          data.created = new Date().toLocaleString();
        }
        // this.$emit("submit", data);
        this.submit2(data);
      }
    },
    closeDialog(arg) {
      if (arg === "edit") {
        this.dialogEdit = false;
      } else {
        this.dialogDelete = false;
      }
    },
  },
  created() {
    this.selectedServer = this.$store.getters["modules/servers/servers"].find(
      (server) => server.id == this.id
    );
  },
};
</script>

<style>
tr :hover {
  cursor: pointer;
}
</style>
