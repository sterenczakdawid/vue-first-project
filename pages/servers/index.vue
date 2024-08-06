<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredServers"
      :server-items-length="totalServers"
      :items-per-page="pageSize"
      :search="search"
      :footer-props="footer"
      @click:row="handleClick"
      @update:options="handlePageChange"
      :loading="isLoading"
    >
      <template v-slot:top>
        <table-toolbar
          :title="$t('servers')"
          :search.sync="search"
          :buttonText="$t('addServer')"
          :openDialog="openDialog"
        />
        <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="'server'">
          <server-form
            :initialData="editedItem"
            @close="close"
            @submit="submit"
          />
        </form-dialog>
        <delete-dialog
          :dialog.sync="dialogDelete"
          :itemName="editedItemName"
          :itemType="'server'"
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
import ServerForm from "~/components/forms/ServerForm.vue";
import TableToolbar from "~/components/ui/TableToolbar.vue";

import { CrudMixin } from "~/mixins/CrudMixin";
import { LocaleMixin } from "~/mixins/LocaleMixin";
import { debounce } from "~/constants/debounce";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    ServerForm,
    TableToolbar,
  },
  mixins: [CrudMixin, LocaleMixin],
  data() {
    return {
      module: "servers",
      itemType: "Server",
      currentPage: null,
      pageSize: null,
      sortBy: "",
      sortDesc: false,
    };
  },
  computed: {
    filteredServers() {
      const searchLower = this.search.toLowerCase();
      return this.items.filter((server) =>
        server.name.toLowerCase().includes(searchLower)
      );
    },
    totalServers() {
      return this.$store.getters["modules/servers/totalServers"];
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
      this.loadServers();
    },
    async loadServers() {
      this.isLoading = true;
      const res = await this.$store.dispatch("modules/servers/loadServers", {
        page: this.currentPage,
        pageSize: this.pageSize,
        sortBy: this.sortBy,
        sortDesc: this.sortDesc,
        ...(this.selectedServer ? { serverId: this.selectedServer } : {}),
        ...(this.selectedApp ? { appId: this.selectedApp } : {}),
        ...(this.search ? { search: this.search } : {}),
      });
      this.isLoading = false;
    },
  },
  watch: {
    search() {
      if (!this.search) this.search = "";
      this.currentPage = 1;
      this.debouncedLoadServers();
    },
  },
  async created() {
    this.debouncedLoadServers = debounce(this.loadServers, 500);
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
</style>
