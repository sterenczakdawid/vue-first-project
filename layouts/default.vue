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
import TheToolbar from "~/components/ui/TheToolbar.vue";
export default {
  components: {
    TheToolbar,
  },
};
</script>

<style lang="scss" scoped></style>
