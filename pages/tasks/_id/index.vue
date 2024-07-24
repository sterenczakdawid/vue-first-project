<template>
  <div>
    <item-details
      :itemType="'Task'"
      :item="this.selectedTask"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <p>
      This task is attached to server:
      <NuxtLink class="link" :to="`/servers/${serverId}`">{{
        serverName
      }}</NuxtLink>
    </p>

    <p v-if="appName">
      This task is attached to application:
      <NuxtLink class="link" :to="`/apps/${appId}`">{{ appName }}</NuxtLink>
    </p>
    <p v-else class="font-italic">
      This task is not attached to any application.
    </p>
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
      :item="this.selectedTask"
      @confirm-delete="deleteItemConfirm"
      @cancel-delete="closeDialog('del')"
    />
  </div>
</template>

<script>
import BackButton from "~/components/utils/BackButton.vue";
import TaskForm from "~/components/tasks/TaskForm.vue";
import FormDialog from "~/components/FormDialog.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import ItemDetails from "~/components/ui/ItemDetails.vue";
export default {
  components: {
    BackButton,
    TaskForm,
    FormDialog,
    DeleteDialog,
    ItemDetails,
  },
  data() {
    return {
      selectedTask: null,
      id: this.$route.params.id,
      editedItem: {},
      dialogEdit: false,
      dialogDelete: false,
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
      return this.selectedTask.name;
    },
    serverId() {
      return this.selectedTask.serverId;
    },
    serverName() {
      return this.servers.find((server) => server.id == this.serverId).name;
    },
    appId() {
      return this.selectedTask.appId;
    },
    appName() {
      if (this.appId) {
        return this.apps.find((app) => app.id == this.appId).name;
      }
    },
    created() {
      return this.selectedTask.created;
    },
    edited() {
      return this.selectedTask.edited;
    },
  },
  created() {
    this.selectedTask = this.$store.getters["modules/tasks/tasks"].find(
      (task) => task.id == this.id
    );
    console.log(this.selectedTask);
    console.log(this.tasks.indexOf(this.selectedTask));
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
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      if (this.id > -1) {
        this.$store.dispatch("modules/tasks/updateTask", {
          index: this.tasks.indexOf(this.selectedTask),
          item: data,
        });
      } else {
        this.$store.dispatch("modules/tasks/addTask", data);
      }
      this.selectedTask = { ...data }; // Aktualizacja selectedTask
      this.dialogEdit = false;
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
