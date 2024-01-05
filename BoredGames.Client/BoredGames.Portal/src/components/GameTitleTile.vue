<script setup>
  import router from '@/router';
  import apiService from '@/api/api';
  import LocalStorageKeys from '@/consts/localStorageKeys';

  const props = defineProps({
    id: Number,
    title: String,
    iconUrl: String
  })

  async function startGame() {
    const response = await apiService.createGame({
      gameTitle: props.id,
      numberOfPlayers: 2,
      requiredNumberOfWins: 1,
      numberOfRounds: 1,
      description: "test",
      playerNickName: "vlado"
    });

    localStorage.setItem(LocalStorageKeys.GameId, response.gameId);
    
    router.push({ name: 'game', params: { gameInstanceId: response.gameId} })
  }
</script>

<template>
  <div class="col-md-4">
    <div style="cursor: pointer">
        <img @click="startGame" v-bind:id="id" width="200" height="250" v-bind:src="iconUrl" v-bind:alt="title"/>
    </div>
    <div>
      <span>{{ title }}</span>
    </div>
  </div>
</template>

<style scoped>
</style>
