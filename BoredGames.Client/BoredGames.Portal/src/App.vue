<script>
import { RouterView } from "vue-router";
import "@/assets/fonts/fonts.css";

export default {
  name: 'app',
  data() {
    return {
      linkToHome: `${import.meta.env.VITE_BASE_URL}`
    }
  }
}
</script>

<template>
  <div class="background">
    <h2 class="header"><a :href="linkToHome">Bored Games</a></h2>
    <RouterView />
  </div>
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
  a {
    color: inherit; 
    text-decoration: inherit;
  }
  div[id="app"] {
    margin: 0;
    padding: 0;
    height: 100%;
    width: 100%;
  }
  .background {
    background-color: #1b2939;
    position:fixed;
    width: 100%;
    height: 100%;
    left: 0;
    top: 0; 
    overflow-y: scroll;
  }
  .header {
    color: whitesmoke;
    text-align: center;
    font-size: 35pt;
  }
</style>
