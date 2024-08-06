<template>
  <v-toolbar flat>
    <v-toolbar-title>{{ title }}</v-toolbar-title>
    <v-divider class="mx-4" inset vertical></v-divider>
    <v-spacer></v-spacer>
    <v-text-field
      v-model="localSearch"
      append-icon="mdi-magnify"
      :label="$t('search')"
      single-line
      hide-details
      clearable
    ></v-text-field>
    <v-spacer></v-spacer>
    <slot name="filters"></slot>
    <v-btn color="primary" @click="openDialog('add')">{{ buttonText }}</v-btn>
  </v-toolbar>
</template>

<script>
export default {
  props: {
    title: String,
    search: String,
    buttonText: String,
    openDialog: Function,
  },
  data() {
    return {
      localSearch: this.search,
    };
  },
  watch: {
    search(newVal) {
      this.localSearch = newVal;
    },
    localSearch(newVal) {
      this.$emit("update:search", newVal);
    },
  },
};
</script>
