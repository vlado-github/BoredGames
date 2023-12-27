<script setup>
import router from '@/router';

  const props = defineProps({
    id: Number,
    title: String,
    iconUrl: String
  })

  function startGame() {
    console.log(`start ${props.id}`);
    router.push({ name: 'play', params: { gameInstanceId: props.id } })
  }
</script>

<template>
  <div>
    <img @click="startGame" v-bind:src="iconUrl" v-bind:id="id" width="50" height="50"/> 
    <span>{{ title }}</span>
  </div>
</template>

<style scoped>
</style>
