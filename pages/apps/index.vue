<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="filteredApps"
      :items-per-page="5"
      :search="search"
      :footer-props="pagination"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>{{ $t("apps") }}</v-toolbar-title>
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
          <v-btn color="primary" @click="openDialog('add')">{{
            $t("addApp")
          }}</v-btn>
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
  </div>
</template>

<script>
import DataTable from "~/components/ui/DataTable.vue";
import DeleteDialog from "~/components/dialogs/DeleteDialog.vue";
import FormDialog from "~/components/dialogs/FormDialog.vue";
import TaskForm from "~/components/forms/TaskForm.vue";
import AppForm from "~/components/forms/AppForm.vue";
import { headers } from "~/constants/headers";
import { pagination } from "~/constants/pagination";
export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    DataTable,
    AppForm,
  },
  data() {
    return {
      headers: headers(this.$i18n),
      pagination: pagination(this.$i18n),
      search: "",
      selectedServer: null,
      dialog: false,
      mode: "add",
      dialogDelete: false,
      editedIndex: -1,
      editedItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: 0,
        tasksIds: [],
      },
      defaultItem: {
        name: "",
        created: null,
        edited: null,
        serverId: -1,
        appId: 0,
        tasksIds: [],
      },
    };
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
    },
    deleteItemConfirm(index) {
      this.$store.dispatch("modules/apps/removeApp", index);
    },
    editItem(item) {
      this.mode = "edit";
      this.editedIndex = this.apps.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch("modules/apps/removeApp", this.editedIndex);
      this.closeDelete();
    },
    closeDialog(isDelete) {
      if (isDelete) {
        this.deleteDialog = false;
      } else {
        this.dialog = false;
      }
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    openDialog(mode) {
      this.mode = mode;
      this.editedItem = Object.assign({}, this.defaultItem);
      this.dialog = true;
    },
    submit(formData) {
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      console.log("data: ", data);
      if (this.editedIndex > -1) {
        this.$store.dispatch("modules/apps/updateApp", {
          index: this.editedIndex,
          item: data,
        });
      } else {
        this.$store.dispatch("modules/apps/addApp", data);
      }
      this.dialog = false;
    },
    localeChanged() {
      this.headers = headers(this.$i18n);
      this.pagination = pagination(this.$i18n);
    },
    setTVar() {
      this.$i18n.locale = this.$store.getters.getLang;
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
.maxw {
  max-width: 300px;
}
</style>
