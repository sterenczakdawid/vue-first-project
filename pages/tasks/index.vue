<template>
  <div class="container">
    <div>Tasks</div>
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
          <v-form class="px-3" ref="form">
            <v-text-field
              label="Name"
              v-model="name"
              :rules="inputRules"
            ></v-text-field>
            <v-menu>
              <template v-slot:activator="{ on }">
                <v-text-field
                  :value="created"
                  v-on="on"
                  label="Created"
                  prepend-icon="mdi-calendar"
                  :rules="[(v) => !!v || 'Pole wymagane']"
                ></v-text-field>
              </template>
              <v-date-picker v-model="created"></v-date-picker>
            </v-menu>
            <v-menu>
              <template v-slot:activator="{ on }">
                <v-text-field
                  :value="edited"
                  v-on="on"
                  label="Edited"
                  prepend-icon="mdi-calendar"
                  :rules="[(v) => !!v || 'Pole wymagane']"
                ></v-text-field>
              </template>
              <v-date-picker v-model="edited"></v-date-picker>
            </v-menu>
            <v-btn class="success mx-0 mt-3" @click="submit">Dodaj</v-btn>
          </v-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
export default {
  data() {
    return {
      name: "",
      created: null,
      edited: null,
      inputRules: [(v) => v.length >= 3 || "Minimalna długość to 3 znaki"],
      dialog: false,
      loading: false,
    };
  },
  methods: {
    submit() {
      if (this.$refs.form.validate()) {
        const formData = {
          name: this.name,
          created: this.created,
          edited: this.edited,
        };

        console.log(formData);
        // Dodanie serwera do Vuex store
        this.$store.dispatch("modules/servers/addServer", formData);

        // Zamknięcie dialogu i zresetowanie formularza
        this.dialog = false;
        this.name = "";
        this.created = null;
        this.edited = null;
      }
    },
    handleClick(item) {
      console.log(item.id);
      this.$router.push("/tasks/" + item.id);
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
      return this.$store.getters["modules/servers/headers"];
    },
  },
};
</script>

<style lang="css" scoped>
.row-pointer >>> tbody tr :hover {
  cursor: pointer;
}
</style>
