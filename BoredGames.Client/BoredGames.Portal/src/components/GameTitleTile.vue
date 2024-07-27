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
      requiredNumberOfWins: 2,
      numberOfRounds: 10,
      description: "test",
      playerNickName: "vlado"
    });

    localStorage.setItem(LocalStorageKeys.GameId, response.gameId);
    
    router.push({ name: 'game', query: { gameInstanceId: response.gameId } })
  }
</script>

<template>
  <div class="col-md-4">
    <div class="title">
        <img @click="startGame" v-bind:id="id" width="250" height="250" v-bind:src="iconUrl" v-bind:alt="title"/>
        <span class="caption">{{ title }}</span>
    </div>
  </div>
</template>

<style scoped>
  .title {
    cursor: pointer;
    text-align: center;
    vertical-align: top;
    display: inline-block;
    text-align: center;
  }
  .caption {
    display: block;
    color: whitesmoke;
}
</style>
