<template>
  <div class="container">
    <h2>Servers</h2>
    <v-data-table
      :headers="headers"
      :items="servers"
      :items-per-page="5"
      class="row-pointer"
      :loading="loading"
      v-if="hasServers"
      @click:row="handleClick"
    ></v-data-table>
    <h2 v-else>Nie ma danych</h2>
    <v-dialog max-width="700px" v-model="dialog">
      <template v-slot:activator="{ on }">
        <v-btn text v-on="on">Dodaj</v-btn>
      </template>
      <v-card>
        <v-card-title>
          <h2>Dodawanie</h2>
        </v-card-title>
        <v-card-text>
          <add-server-form
            @save-data="saveData"
            @update-dialog="updateDialog"
          />
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import AddServerForm from "~/components/servers/AddServerForm.vue";
export default {
  components: {
    AddServerForm,
  },
  data() {
    return {
      inputRules: [(v) => v.length >= 3 || "Minimalna długość to 3 znaki"],
      dialog: false,
      loading: false,
    };
  },
  methods: {
    handleClick(item) {
      console.log(item.id);
      this.$router.push(this.$route.path + "/" + item.id);
    },
    saveData(formData) {
      this.$store.dispatch("modules/servers/addServer", formData);
    },
    updateDialog(value) {
      this.dialog = value;
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
