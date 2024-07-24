<!-- <template>
  <v-data-table
    :headers="headers"
    :items="items"
    :items-per-page="5"
    @click:row="handleClick"
  >
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
        <v-divider class="mx-4" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="$emit('openDialog', 'add')">{{
          addButtonLabel
        }}</v-btn>
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
</template>

<script>
export default {
  props: {
    headers: Array,
    items: Array,
    title: String,
    addButtonLabel: String,
  },
  methods: {
    handleClick(item) {
      this.$router.push(this.$route.path + "/" + item.id);
    },
    editItem(item) {
      this.$emit("editItem", item);
    },
    deleteItem(item) {
      this.$emit("deleteItem", item);
    },
  },
};
</script>

<style></style> -->
<!-- components/DataTable.vue -->
<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="items"
      :items-per-page="5"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>{{ title }}</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="openDialog('add')"
            >add new {{ itemType }}</v-btn
          >
          <form-dialog :dialog.sync="dialog" :mode="mode" :itemType="itemType">
            <task-form
              v-if="itemType === 'task'"
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            ></task-form>
            <ServerForm
              v-else-if="itemType === 'server'"
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            />
            <AppForm
              v-else
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            ></AppForm>
            <!--
              <component
              :is="TaskForm"
              ref="itemForm"
              :initialData="editedItem"
              @close="close"
              @submit="submit"
            />
            -->
          </form-dialog>
          <!-- <delete-dialog
            :dialog.sync="dialogDelete"
            :itemName="editedItemName"
            @confirm-delete="deleteItemConfirm"
            @cancel-delete="closeDelete"
          /> -->
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
import DeleteDialog from "~/components/servers/DeleteDialog.vue";
import FormDialog from "~/components/FormDialog.vue";
import TaskForm from "./tasks/TaskForm.vue";
import ServerForm from "./servers/ServerForm.vue";
import AppForm from "./apps/AppForm.vue";

export default {
  components: {
    DeleteDialog,
    FormDialog,
    TaskForm,
    ServerForm,
    AppForm,
  },
  props: {
    title: String,
    items: Array,
    itemType: String,
  },
  data() {
    return {
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
    editItem(item) {
      console.log("edytowanie itemu: ", item);
      this.mode = "edit";
      this.editedIndex = this.items.indexOf(item);
      this.editedItem = Object.assign({}, item);
      console.log(
        "przypisano - edited item: " +
          this.editedItem.name +
          ", editedIndex: " +
          this.editedIndex
      );
      this.dialog = true;
    },
    deleteItem(item) {
      this.editedIndex = item.id;
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteItemConfirm() {
      this.$store.dispatch(
        `modules/${this.storeModule}/removeItem`,
        this.editedIndex
      );
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
      console.log("submit w DataTable");
      const data = {
        ...formData,
        edited: new Date().toLocaleString(),
      };
      if (!formData.created) {
        data.created = new Date().toLocaleString();
      }
      console.log("Po dodaniu created i/lub edited w data table", data);
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
  },
  computed: {
    hasItems() {
      return this.$store.getters[`modules/${this.storeModule}/hasItems`];
    },
    headers() {
      return this.$store.getters.getHeaders;
    },
    editedItemName() {
      return this.editedItem.name;
    },
  },
};
</script>

<style lang="css">
tr :hover {
  cursor: pointer;
}
</style>
