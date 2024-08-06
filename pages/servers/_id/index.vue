<template>
  <div v-if="isLoading">
    <v-progress-circular indeterminate></v-progress-circular>
  </div>
  <div v-else>
    <item-details
      :itemType="'Server'"
      :item="selectedServer"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <v-tabs v-model="tab" grow>
      <v-tab>{{ $t("apps") }}</v-tab>
      <v-tab>{{ $t("tasks") }}</v-tab>
    </v-tabs>
    <v-tabs-items v-model="tab">
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredApps"
          :items-per-page="5"
          :footer-props="footer"
          class="row-pointer"
          @click:row="handleClickApp"
        ></v-data-table
      ></v-tab-item>
      <v-tab-item>
        <v-data-table
          :headers="headers"
          :items="filteredTasks"
          :items-per-page="5"
          :footer-props="footer"
          class="row-pointer"
          @click:row="handleClickTask"
        ></v-data-table
      ></v-tab-item>
    </v-tabs-items>
    <delete-dialog
      :dialog.sync="dialogDelete"
      :itemName="name"
      :itemType="'server'"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
    <server-form-dialog
      :dialog.sync="dialogEdit"
      :mode="'edit'"
      :itemType="'server'"
      ><ServerForm
        ref="serverForm"
        :initialData="selectedServer"
        @close="closeDialog('edit')"
        @submit="submit"
    /></server-form-dialog>
    <back-button />
  </div>
</template>

<script>
import BackButton from "~/components/ui/BackButton.vue";
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import ServerFormDialog from "~/components/dialogs/FormDialog.vue";
import ServerForm from "~/components/forms/ServerForm.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";

import { LocaleMixin } from "~/mixins/LocaleMixin";
export default {
  components: {
    BackButton,
    DeleteDialog,
    ServerFormDialog,
    ServerForm,
    ItemDetails,
  },
  mixins: [LocaleMixin],
  data() {
    return {
      dialogDelete: false,
      dialogEdit: false,
      editedItem: {},
      tab: null,
      selectedServer: null,
      id: this.$route.params.id,
      isLoading: true,
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    name() {
      return this.selectedServer ? this.selectedServer.name : "";
    },
    created() {
      return this.selectedServer.created;
    },
    edited() {
      return this.selectedServer.edited;
    },
    filteredTasks() {
      if (!this.selectedServer) return [];
      return this.tasks.filter(
        (task) => task.serverId === this.selectedServer.id
      );
    },
    filteredApps() {
      if (!this.selectedServer) return [];
      return this.apps.filter((app) => app.serverId === this.selectedServer.id);
    },
  },
  methods: {
    handleClickTask(item) {
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
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      if (this.id > -1) {
        this.$store.dispatch("modules/servers/updateServer", {
          index: this.servers.indexOf(this.selectedServer),
          item: data,
        });
      } else {
        this.$store.dispatch("modules/servers/addServer", data);
      }
      this.selectedServer = { ...data };
      this.dialogEdit = false;
    },
    closeDialog(arg) {
      if (arg === "edit") {
        this.dialogEdit = false;
      } else {
        this.dialogDelete = false;
      }
    },
    async loadServers() {
      this.isLoading = true;
      await this.$store.dispatch("modules/apps/loadApps", {
        pageSize: -1,
      });
      await this.$store.dispatch("modules/servers/loadServers", {
        pageSize: -1,
      });
      await this.$store.dispatch("modules/tasks/loadTasks", {
        pageSize: -1,
      });
      this.selectedServer = this.servers.find((server) => server.id == this.id);
      this.isLoading = false;
    },
  },
  created() {
    this.loadServers();
  },
  watch: {
    "$route.params.id": {
      immediate: true,
      handler(newId) {
        this.id = newId;
        this.loadServers();
      },
    },
    servers: {
      immediate: true,
      handler() {
        this.selectedServer = this.servers.find(
          (server) => server.id == this.id
        );
      },
    },
  },
};
</script>

<style>
tr :hover {
  cursor: pointer;
}
</style>
