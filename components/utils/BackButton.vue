<template>
  <v-btn @click="goBack">Back</v-btn>
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
