<!-- <template>
  <v-app dark>
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
    <v-footer :absolute="!fixed" app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>
-->
<template>
  <v-app>
    <the-toolbar />
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import TheToolbar from "~/components/TheToolbar.vue";
export default {
  components: {
    TheToolbar,
  },
};
</script>

<style lang="scss" scoped></style>
