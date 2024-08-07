<template>
  <div class="container">
    <v-snackbar v-model="snackbar" :color="snackbarColor" top elevation="24">
      {{ snackbarMessage }}
    </v-snackbar>
    <v-data-table
      :headers="headers"
      :items="filteredApps"
      :server-items-length="totalApps"
      :items-per-page="pageSize"
      :search="search"
      :footer-props="footer"
      @click:row="handleClick"
      @update:options="handlePageChange"
      :loading="isLoading"
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
            <v-spacer></v-spacer>
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
    <v-btn @click="downloadExcel" color="success" outlined class="excel">
      Excel
    </v-btn>
  </div>
</template>

<script>
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import TaskForm from "~/components/forms/TaskForm.vue";
import AppForm from "~/components/forms/AppForm.vue";
import TableToolbar from "~/components/ui/TableToolbar.vue";
import { CrudMixin } from "~/mixins/CrudMixin";
import { LocaleMixin } from "~/mixins/LocaleMixin";
import { debounce } from "~/constants/debounce";
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
      currentPage: null,
      pageSize: null,
      debouncedLoadApps: null,
    };
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    apps() {
      return this.$store.getters["modules/apps/apps"];
    },
    totalApps() {
      return this.$store.getters["modules/apps/totalApps"];
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
  methods: {
    handlePageChange(options) {
      this.currentPage = options.page;
      this.pageSize = options.itemsPerPage;
      this.sortBy = options.sortBy.length
        ? capitalizeFirstLetter(options.sortBy[0])
        : "";
      this.sortDesc = options.sortDesc[0];
      this.loadApps();
    },
    async loadApps() {
      this.isLoading = true;
      await this.$store.dispatch("modules/apps/loadApps", {
        page: this.currentPage,
        pageSize: this.pageSize,
        sortBy: this.sortBy,
        sortDesc: this.sortDesc,
        ...(this.selectedServer ? { serverId: this.selectedServer } : {}),
        ...(this.search ? { search: this.search } : {}),
      });
      this.isLoading = false;
    },
    async downloadExcel() {
      await this.$store.dispatch("modules/apps/exportToExcel");
    },
  },
  watch: {
    selectedServer() {
      this.currentPage = 1;
      this.loadApps();
    },
    search() {
      if (!this.search) this.search = "";
      this.currentPage = 1;
      this.debouncedLoadApps();
    },
  },
  async created() {
    this.debouncedLoadApps = debounce(this.loadApps, 500);
    await this.loadAllServers();
    await this.loadAllTasks();
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
