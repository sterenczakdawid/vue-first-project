<template>
  <div class="container">
    <v-data-table
      :headers="headers"
      :items="servers"
      :items-per-page="5"
      class="row-pointer"
      :loading="loading"
      v-if="hasServers"
      @click:row="handleClick"
    >
      <template v-slot:top>
        <v-toolbar>
          <v-toolbar-title>Servers</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="openDialog">Add new server</v-btn>
        </v-toolbar>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small class="mr-2" @click.stop="editItem(item)">
          mdi-pencil
        </v-icon>
        <v-icon small @click.stop="deleteItem(item)"> mdi-delete </v-icon>
      </template>
    </v-data-table>
    <!-- <h2 v-else>Nie ma danych</h2> -->

    <server-form-dialog v-model="dialog" @save-data="saveData" />
  </div>
</template>

<script>
import ServerFormDialog from "~/components/servers/ServerFormDialog.vue";
export default {
  components: {
    ServerFormDialog,
  },
  data() {
    return {
      dialog: false,
      loading: false,
    };
  },
  methods: {
    handleClick(item) {
      // console.log(item.id);
      this.$router.push(this.$route.path + "/" + item.id);
    },
    saveData(formData) {
      this.$store.dispatch("modules/servers/addServer", formData);
      this.dialog = false;
    },
    updateDialog(value) {
      this.dialog = value;
    },
    openDialog() {
      this.dialog = true;
    },
    editItem(item) {
      console.log("editing " + item.name);
    },
    deleteItem(item) {
      console.log("deleting " + item.name);
      this.$store.dispatch("modules/servers/removeServer", item.id);
    },
  },
  computed: {
    servers() {
      return this.$store.getters["modules/servers/servers"];
    },
    hasServers() {
      return this.$store.getters["modules/servers/hasServers"];
    },
    headers() {
      return this.$store.getters.getHeaders;
    },
  },
};
</script>

<style lang="css">
.row-pointer >>> tbody tr :hover {
  cursor: pointer;
}
</style>
