<template>
  <div v-if="isLoading">
    <v-progress-circular indeterminate></v-progress-circular>
  </div>
  <div v-else>
    <item-details
      :itemType="'Application'"
      :item="this.selectedApp"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <p>
      {{ $t("appAttached") }}:
      <NuxtLink class="link" :to="`/servers/${serverId}`">{{
        serverName
      }}</NuxtLink>
    </p>
    <v-data-table
      :headers="detailsHeaders"
      :items="filteredTasks"
      :items-per-page="5"
      :footer-props="footer"
      class="row-pointer"
      @click:row="handleClick"
    ></v-data-table>
    <back-button />
    <form-dialog :dialog.sync="dialogEdit" :mode="'edit'" :itemType="'app'">
      <app-form
        :initialData="this.selectedApp"
        @close="closeDialog('edit')"
        @submit="submit"
      ></app-form>
    </form-dialog>
    <delete-dialog
      :dialog.sync="dialogDelete"
      :itemName="name"
      :itemType="'app'"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
  </div>
</template>

<script>
import BackButton from "~/components/ui/BackButton.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";
import AppForm from "~/components/forms/AppForm.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";

import { LocaleMixin } from "~/mixins/LocaleMixin";
export default {
  components: {
    BackButton,
    ItemDetails,
    AppForm,
    FormDialog,
    DeleteDialog,
  },
  mixins: [LocaleMixin],
  data() {
    return {
      selectedApp: null,
      id: this.$route.params.id,
      editedItem: {},
      dialogEdit: false,
      dialogDelete: false,
      isLoading: true,
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    serverId() {
      return this.selectedApp.serverId;
    },
    serverName() {
      const server = this.servers.find((server) => server.id == this.serverId);
      return server ? server.name : "";
    },
    name() {
      return this.selectedApp ? this.selectedApp.name : "";
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    filteredTasks() {
      if (!this.selectedApp) return [];
      return this.tasks.filter((task) => task.appId === this.selectedApp.id);
    },
  },
  methods: {
    handleClick(item) {
      this.$router.push("/tasks/" + item.id);
    },
    editItem() {
      this.editedItem = Object.assign({}, this.selectedApp);
      this.dialogEdit = true;
    },
    deleteItem() {
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/apps/removeApp", this.id);
      this.closeDialog("del");
      this.$router.replace("/apps");
    },
    closeDialog(arg) {
      if (arg === "edit") {
        this.dialogEdit = false;
      } else {
        this.dialogDelete = false;
      }
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
        this.$store.dispatch("modules/apps/updateApp", {
          index: this.apps.indexOf(this.selectedApp),
          item: data,
        });
      } else {
        this.$store.dispatch("modules/apps/addApp", data);
      }
      this.selectedApp = { ...data };
      this.dialogEdit = false;
    },
    async loadApps() {
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
      this.selectedApp = this.apps.find((app) => app.id == this.id);
      this.isLoading = false;
    },
  },
  created() {
    this.loadApps();
  },
  watch: {
    "$route.params.id": {
      immediate: true,
      handler(newId) {
        this.id = newId;
        this.loadApps();
      },
    },
    apps: {
      immediate: true,
      handler() {
        this.selectedApp = this.apps.find((app) => app.id == this.id);
      },
    },
  },
};
</script>

<style></style>
