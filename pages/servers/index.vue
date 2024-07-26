<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredServers"
      :items-per-page="5"
      :search="search"
      :footer-props="pagination"
      v-if="hasServers"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>{{ $t("servers") }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            :label="$t('search')"
            single-line
            hide-details
          ></v-text-field>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="openDialog('add')">{{
            $t("addServer")
          }}</v-btn>
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
      :headers="headers"
      :items="servers"
      :itemType="'server'"
      :title="'costam'"
      :addButtonLabel="'lol'"
      @submit="submit"
      @openDialog="openDialog"
      @editItem="editItem"
      @deleteItem="deleteItem"
    >
    </data-table> -->
  </div>
</template>

<script>
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import ServerForm from "~/components/forms/ServerForm.vue";
import { headers } from "~/constants/headers";
import { pagination } from "~/constants/pagination";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    ServerForm,
  },
  data() {
    return {
      pagination: pagination(this.$i18n),
      headers: headers(this.$i18n),
      search: "",
      dialog: false,
      mode: "add",
      dialogDelete: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        tasksIds: [],
        appsIds: [],
      },
    };
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
    },
    editItem(item) {
      this.mode = "edit";
      this.editedIndex = this.servers.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/servers/removeServer", this.editedIndex);
      this.closeDelete();
    },
    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/servers/updateServer", {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch("modules/servers/addServer", data);
      }
      this.dialog = false;
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
    localeChanged() {
      this.headers = headers(this.$i18n);
    },
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLang;
    },
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    hasServers() {
      return this.$store.getters["modules/servers/hasServers"];
    },
    filteredServers() {
      const searchLower = this.search.toLowerCase();
      return this.servers.filter((server) =>
        server.name.toLowerCase().includes(searchLower)
      );
    },
    editedItemName() {
      return this.editedItem.name;
    },
  },
  watch: {
    "$i18n.locale": "localeChanged",
    "$store.getters.getLang": "setTVar",
  },
  beforeRouteEnter(_, from, next) {
    next((vm) => {
      vm.setTVar();
    });
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
</style>
