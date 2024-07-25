<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredTasks"
      :items-per-page="5"
      :search="search"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Tasks</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            hide-details
          ></v-text-field>
          <v-spacer></v-spacer>
          <v-select
            v-model="selectedServer"
            :items="servers"
            item-text="name"
            item-value="id"
            label="Select server"
            hide-details
            clearable
            class="lol"
          ></v-select>
          <v-spacer></v-spacer>
          <v-select
            v-model="selectedApp"
            :items="filteredApps"
            item-text="name"
            item-value="id"
            label="Select app"
            hide-details
            single-line
            class="lol"
            clearable
          ></v-select>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="openDialog('add')">add new task</v-btn>
          <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="'task'">
            <task-form
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            ></task-form>
          </form-dialog>
          <delete-dialog
            :dialog.sync="dialogDelete"
            :itemName="editedItemName"
            :itemType="'task'"
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
    <!-- <data-table
      :items="tasks"
      :itemType="'task'"
      :title="'Tasks'"
      @submit="submit"
      @openDialog="openDialog"
      @editItem="editItem"
      @deleteItem="deleteItem"
    >
    </data-table> -->
  </div>
</template>

<script>
import DataTable from "~/components/DataTable.vue";
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import FormDialog from "~/components/FormDialog.vue";
import TaskForm from "~/components/tasks/TaskForm.vue";
import TaskFormDialog from "~/components/tasks/TaskFormDialog.vue";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    DataTable,
    TaskFormDialog,
  },
  data() {
    return {
      search: "",
      selectedServer: null,
      selectedApp: null,
      dialog: false,
      mode: "add",
      dialogDelete: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: -1,
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: -1,
      },
    };
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
    },
    deleteItemConfirm(index) {
      this.$store.dispatch("modules/tasks/removeTask", index);
    },
    editItem(item) {
      this.mode = "edit";
      this.editedIndex = this.tasks.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/tasks/removeTask", this.editedIndex);
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
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    openDialog(mode) {
      this.mode = mode;
      this.dialog = true;
    },
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      console.log(formData);
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/tasks/updateTask", {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch("modules/tasks/addTask", data);
      }
      this.dialog = false;
    },
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    tasks() {
      return this.$store.getters["modules/tasks/tasks"];
    },
    headers() {
      return this.$store.getters.getHeaders;
    },
    editedItemName() {
      return this.editedItem.name;
    },
    filteredTasks() {
      return this.tasks.filter((task) => {
        const serverMatch = this.selectedServer
          ? task.serverId === this.selectedServer
          : true;
        const appMatch = this.selectedApp
          ? task.appId === this.selectedApp
          : true;
        const nameMatch = task.name
          .toLowerCase()
          .includes(this.search.toLowerCase());
        return serverMatch && appMatch && nameMatch;
      });
    },
    filteredApps() {
      return this.selectedServer
        ? this.apps.filter((app) => app.serverId === this.selectedServer)
        : this.apps;
    },
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
.lol {
  max-width: 230px;
}
</style>
