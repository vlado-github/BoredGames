<script setup>
import { RouterView } from "vue-router";
import "@/assets/fonts/fonts.css";
</script>

<template>
  <RouterView />
</template>

<style>
html,
body {
  font-family: "DynaPuff", sans-serif;
  margin: 0;
  padding: 0;
  height: 100%;
  width: 100%;
}
div[id="app"] {
  margin: 0;
  padding: 0;
  height: 100%;
  width: 100%;
}
</style>
