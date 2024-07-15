<template>
  <div>
    <div class="d-flex justify-space-between align-center py-8">
      <div>
        <h2>{{ name }} - details</h2>
        <p class="grey--text text--lighten-1 ma-0">Created: {{ created }}</p>
        <p class="grey--text text--lighten-1">Edited: {{ edited }}</p>
      </div>
      <div class="d-flex flex-column">
        <v-btn class="mb-2" width="125px" @click="editItem()">Edit</v-btn>
        <v-btn class="mt-2" color="error" width="125px" @click="deleteItem()"
          >Delete</v-btn
        >
      </div>
    </div>
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
      @cancel-delete="closeDelete"
    />
    <server-form-dialog
      :dialog.sync="dialogEdit"
      @close="closeEdit"
      @submit="editItemSubmit"
      :mode="'edit'"
      :editedItem="this.editedItem"
    ></server-form-dialog>
    <back-button />
  </div>
</template>

<script>
import BackButton from "~/components/utils/BackButton.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import ServerFormDialog from "~/components/servers/ServerFormDialog.vue";
export default {
  components: {
    BackButton,
    DeleteDialog,
    ServerFormDialog,
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
      this.closeDelete();
      this.$router.replace("/servers");
    },
    closeDelete() {
      this.dialogDelete = false;
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
    closeEdit() {
      this.dialogEdit = false;
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
