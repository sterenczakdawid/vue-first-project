<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredServers"
      :items-per-page="5"
      :search="search"
      :footer-props="footer"
      @click:row="handleClick"
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
    };
  },
  computed: {
    filteredServers() {
      const searchLower = this.search.toLowerCase();
      return this.items.filter((server) =>
        server.name.toLowerCase().includes(searchLower)
      );
    },
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
</style>
