<template>
  <div>
    <item-details
      :itemType="'Server'"
      :item="this.selectedServer"
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
          @click:row="handleClick"
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
        :initialData="this.selectedServer"
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
import { detailsHeaders } from "~/constants/headers";
import { footer } from "~/constants/footer";
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
      footer: footer(this.$i18n),
      headers: detailsHeaders(this.$i18n),
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
      return this.tasks.filter(
        (task) => task.serverId === this.selectedServer.id
      );
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    filteredApps() {
      if (!this.selectedServer) return [];
      return this.apps.filter((app) => app.serverId === this.selectedServer.id);
    },
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
  },
  methods: {
    handleClick(item) {
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
    localeChanged() {
      this.headers = detailsHeaders(this.$i18n);
      this.footer = footer(this.$i18n);
    },
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLang;
    },
    loadTasks() {
      this.$store.dispatch("modules/servers/loadServers");
      this.$store.dispatch("modules/tasks/loadTasks");
      // this.$store.dispatch("modules/apps/loadApps");
    },
  },
  async created() {
    this.loadTasks();
    this.selectedServer = this.$store.getters["modules/servers/servers"].find(
      (server) => server.id == this.id
    );
  },
  watch: {
    "$i18n.locale": "localeChanged",
    "$store.getters.getLang": "setTVar",
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
    });
  },
};
</script>

<style>
tr :hover {
  cursor: pointer;
}
</style>
