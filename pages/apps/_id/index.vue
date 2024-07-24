<template>
  <div>
    <item-details
      :itemType="'Application'"
      :item="this.selectedApp"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <p>
      This app is attached to server:
      <NuxtLink class="link" :to="`/servers/${serverId}`">{{
        serverName
      }}</NuxtLink>
    </p>
    <v-data-table
      :headers="headers"
      :items="filteredTasks"
      :items-per-page="5"
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
      :item="this.selectedApp"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
  </div>
</template>

<script>
import BackButton from "~/components/utils/BackButton.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";
import AppForm from "~/components/apps/AppForm.vue";
import FormDialog from "~/components/FormDialog.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
export default {
  components: {
    BackButton,
    ItemDetails,
    AppForm,
    FormDialog,
    DeleteDialog,
  },
  data() {
    return {
      selectedApp: null,
      id: this.$route.params.id,
      editedItem: {},
      dialogEdit: false,
      dialogDelete: false,
    };
  },
  computed: {
    headers() {
      return this.$store.getters.getHeaders;
    },
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
      return this.servers.find((server) => server.id == this.serverId).name;
    },
    name() {
      return this.selectedApp.name;
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
      this.editedItem = Object.assign({}, this.selectedTask);
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
  },
  created() {
    this.selectedApp = this.$store.getters["modules/apps/apps"].find(
      (app) => app.id == this.id
    );
  },
};
</script>

<style></style>
