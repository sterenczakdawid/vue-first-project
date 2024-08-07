<template>
  <div v-if="isLoading">
    <v-progress-circular indeterminate></v-progress-circular>
  </div>
  <div v-else>
    <v-snackbar v-model="snackbar" :color="snackbarColor" top elevation="24">
      {{ snackbarMessage }}
    </v-snackbar>
    <item-details
      :itemType="'Task'"
      :item="this.selectedTask"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <p>
      {{ $t("taskAttachedServer") }}:
      <NuxtLink class="link" :to="`/servers/${serverId}`">{{
        serverName
      }}</NuxtLink>
    </p>

    <p v-if="appName">
      {{ $t("taskAttachedApp") }}:
      <NuxtLink class="link" :to="`/apps/${appId}`">{{ appName }}</NuxtLink>
    </p>
    <p v-else class="font-italic">{{ $t("taskNotAttachedApp") }}.</p>
    <back-button />
    <form-dialog :dialog.sync="dialogEdit" :mode="'edit'" :itemType="'task'">
      <task-form
        ref="taskForm"
        :initialData="this.selectedTask"
        @close="closeDialog('edit')"
        @submit="submit"
      ></task-form>
    </form-dialog>
    <delete-dialog
      :dialog.sync="dialogDelete"
      :itemName="name"
      :itemType="'task'"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
  </div>
</template>

<script>
import BackButton from "~/components/ui/BackButton.vue";
import TaskForm from "~/components/forms/TaskForm.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";

import { LocaleMixin } from "~/mixins/LocaleMixin";
export default {
  components: {
    BackButton,
    TaskForm,
    FormDialog,
    DeleteDialog,
    ItemDetails,
  },
  mixins: [LocaleMixin],
  data() {
    return {
      selectedTask: null,
      id: this.$route.params.id,
      editedItem: {},
      dialogEdit: false,
      dialogDelete: false,
      isLoading: true,
      snackbar: false,
      snackbarMessage: "",
      snackbarColor: "error",
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
      return this.selectedTask ? this.selectedTask.name : "";
    },
    serverId() {
      return this.selectedTask ? this.selectedTask.serverId : null;
    },
    serverName() {
      const server = this.servers.find((server) => server.id == this.serverId);
      return server ? server.name : "";
    },
    appId() {
      return this.selectedTask ? this.selectedTask.appId : null;
    },
    appName() {
      const app = this.apps.find((app) => app.id == this.appId);
      return app ? app.name : "";
    },
  },
  methods: {
    editItem() {
      this.editedItem = Object.assign({}, this.selectedTask);
      this.dialogEdit = true;
    },
    deleteItem() {
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/tasks/removeTask", this.id);
      this.closeDialog("del");
      this.$router.replace("/tasks");
    },
    closeDialog(arg) {
      if (arg === "edit") {
        this.dialogEdit = false;
      } else {
        this.dialogDelete = false;
      }
    },
    async submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString().slice(0, -3),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString().slice(0, -3);
      }
      try {
        if (this.id > -1) {
          await this.$store.dispatch("modules/tasks/updateTask", {
            index: this.tasks.indexOf(this.selectedTask),
            item: data,
          });
        } else {
          await this.$store.dispatch("modules/tasks/addTask", data);
        }
        this.selectedTask = { ...data };
        this.dialogEdit = false;
        this.snackbarColor = "success";
        this.snackbarMessage = this.$t("snackbars.saveSuccess");
        this.snackbar = true;
      } catch (error) {
        this.snackbarColor = "error";
        this.snackbarMessage =
          `${this.$t("snackbars.saveError")}: ` + error.message;
        this.snackbar = true;
      }
    },
    async loadTasks() {
      this.isLoading = true;
      await this.$store.dispatch("modules/servers/loadServers", {
        pageSize: -1,
      });
      await this.$store.dispatch("modules/apps/loadApps", {
        pageSize: -1,
      });
      await this.$store.dispatch("modules/tasks/loadTasks", {
        pageSize: -1,
      });
      this.selectedTask = this.tasks.find((task) => task.id == this.id);
      this.isLoading = false;
    },
  },
  created() {
    this.loadTasks();
  },
  watch: {
    "$route.params.id": {
      immediate: true,
      handler(newId) {
        this.id = newId;
        this.loadTasks();
      },
    },
    tasks: {
      immediate: true,
      handler() {
        this.selectedTask = this.tasks.find((task) => task.id == this.id);
      },
    },
  },
};
</script>

<style scoped>
.link {
  font-size: 20px;
  text-decoration: none;
}

.link:hover {
  text-decoration: underline;
}
</style>
