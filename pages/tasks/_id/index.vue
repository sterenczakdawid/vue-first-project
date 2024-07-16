<template>
  <div>
    <item-details
      :itemType="'Task'"
      :item="this.selectedTask"
      @editItem="editItem"
      @deleteItem="deleteItem"
    ></item-details>
    <p>{{ serverId }}. {{ serverName }}</p>
    <p>{{ appId }}. {{ appName }}</p>
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
      return this.apps.find((server) => server.id == this.serverId).app;
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
  },
};
</script>

<style></style>
