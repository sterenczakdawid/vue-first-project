<template>
  <v-btn class="back-btn" @click="goBack">{{ $t("buttons.back") }}</v-btn>
</template>

<script>
export default {
  methods: {
    goBack() {
      this.$router.back();
    },
  },
};
</script>

<style>
.back-btn {
  margin-top: 10px;
}
</style>
