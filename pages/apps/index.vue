<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredApps"
      :items-per-page="5"
      :search="search"
      :footer-props="footer"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <table-toolbar
          :title="$t('apps')"
          :search.sync="search"
          :buttonText="$t('addApp')"
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
              clearable
              class="maxw"
            ></v-select>
          </template>
        </table-toolbar>
        <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="'app'">
          <app-form
            :initialData="editedItem"
            @close="closeDialog(false)"
            @submit="submit"
          ></app-form>
        </form-dialog>
        <delete-dialog
          :dialog.sync="dialogDelete"
          :itemName="editedItemName"
          :itemType="'app'"
          @confirm-delete="deleteItemConfirm"
          @cancel-delete="closeDialog(true)"
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
import AppForm from "~/components/forms/AppForm.vue";
import TableToolbar from "~/components/ui/DataTable/TableToolbar.vue";
import { CrudMixin } from "~/mixins/CrudMixin";
import { LocaleMixin } from "~/mixins/LocaleMixin";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    AppForm,
    TableToolbar,
  },
  mixins: [CrudMixin, LocaleMixin],
  data() {
    return {
      module: "apps",
      itemType: "App",
      selectedServer: null,
    };
  },
  methods: {
    loadApps() {
      this.$store.dispatch("modules/apps/loadApps");
      this.$store.dispatch("modules/tasks/loadTasks");
      this.$store.dispatch("modules/servers/loadServers");
    },
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    filteredApps() {
      return this.apps.filter((app) => {
        const serverMatch = this.selectedServer
          ? app.serverId === this.selectedServer
          : true;
        const nameMatch = app.name
          .toLowerCase()
          .includes(this.search.toLowerCase());
        return serverMatch && nameMatch;
      });
    },
  },
  created() {
    this.loadApps();
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
.maxw {
  max-width: 300px;
}
</style>
