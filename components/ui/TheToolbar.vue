<template>
  <div>
    <v-navigation-drawer app v-model="drawer" temporary>
      <v-list>
        <v-list-item :to="home.to" exact
          ><v-list-item-action>
            <v-icon>{{ home.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>{{ home.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-divider class="blue darken-2 pt-1 my-1" />
        <v-list-item v-for="(item, i) in items" :key="i" :to="item.to" exact>
          <v-list-item-action>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-app-bar app>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <NuxtLink to="/" class="text-decoration-none white--text pa-5">
        <v-toolbar-title>{{ $t("title") }}</v-toolbar-title>
      </NuxtLink>
      <v-spacer></v-spacer>
      <v-btn-toggle v-model="lang_toggle">
        <v-btn @click="changeLang('pl')"> PL </v-btn>
        <v-btn @click="changeLang('en')"> EN </v-btn>
      </v-btn-toggle>
    </v-app-bar>
  </div>
</template>

<script>
export default {
  data() {
    return {
      lang_toggle: null,
      drawer: false,
      home: { icon: "mdi-home", title: this.$t("home"), to: "/" },
      items: [
        {
          icon: "mdi-server",
          title: this.$t("servers"),
          to: "/servers",
        },
        {
          icon: "mdi-application-outline",
          title: this.$t("apps"),
          to: "/apps",
        },
        {
          icon: "mdi-file-tree",
          title: this.$t("tasks"),
          to: "/tasks",
        },
      ],
    };
  },
  methods: {
    changeLang(lang) {
      this.$i18n.locale = lang;
      this.$store.dispatch("setLang", lang);
    },
  },
};
</script>
