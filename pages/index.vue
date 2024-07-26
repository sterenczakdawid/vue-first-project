<template>
  <div class="home-container">
    <h2>{{ $t("home") }}</h2>
    <NuxtLink to="/servers">
      <v-btn color="primary" width="200px">{{ $t("servers") }}</v-btn>
    </NuxtLink>
    <NuxtLink to="/apps">
      <v-btn color="primary" width="200px">{{ $t("apps") }}</v-btn>
    </NuxtLink>
    <NuxtLink to="/tasks">
      <v-btn color="primary" width="200px">{{ $t("tasks") }}</v-btn>
    </NuxtLink>
  </div>
</template>

<script>
export default {
  name: "IndexPage",
};
</script>

<style scoped>
.home-container {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  gap: 20px;
}
</style>
