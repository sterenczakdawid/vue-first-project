<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredTasks"
      :items-per-page="5"
      :search="search"
      :footer-props="footer"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <table-toolbar
          :title="$t('tasks')"
          :search.sync="search"
          :buttonText="$t('addTask')"
          :openDialog="openDialog"
        >
          <template v-slot:filters>
            <v-select
              v-model="selectedServer"
              :items="servers"
              item-text="name"
              item-value="id"
              :label="$t('selectServer')"
              hide-details
              single-line
              clearable
            ></v-select>
            <v-spacer></v-spacer>
            <v-select
              v-model="selectedApp"
              :items="filteredApps"
              item-text="name"
              item-value="id"
              :label="$t('selectApp')"
              hide-details
              single-line
              class="lol"
              clearable
            ></v-select>
          </template>
        </table-toolbar>
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
  </div>
</template>

<script>
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import TaskForm from "~/components/forms/TaskForm.vue";
import TableToolbar from "~/components/ui/DataTable/TableToolbar.vue";
import { LocaleMixin } from "~/mixins/LocaleMixin";
import { CrudMixin } from "~/mixins/CrudMixin";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    TableToolbar,
  },
  mixins: [CrudMixin, LocaleMixin],
  data() {
    return {
      module: "tasks",
      itemType: "Task",
      selectedServer: null,
      selectedApp: null,
    };
  },
  methods: {
    loadTasks() {
      this.$store.dispatch("modules/servers/loadServers");
      this.$store.dispatch("modules/tasks/loadTasks");
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
  created() {
    this.loadTasks();
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
