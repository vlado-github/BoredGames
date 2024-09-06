<script>
import GameTitleTile from './GameTitleTile.vue'
import apiService from '../api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: 'gameGallery',
  components: {
    GameTitleTile
  },

  data() {
    return {
      titles: [],
      playerName: ''
    };
  },

  mounted() {
    this.fetchTitles();
    this.playerName = localStorage.getItem(LocalStorageKeys.PlayerNickName);
  },

  methods: {
    async fetchTitles() {
      this.titles = await apiService.getTitles();
    }
  }
}
</script>

<template>
  <div class="background">
    <h2 class="header">Bored Games</h2>
    <div class="gallery">
      <div class="placement">
        <GameTitleTile
          v-for="title in titles"
          :titleId="title.id"
          :titleName="title.name"
          :iconUrl="title.thumbnailImageUrl"
          :formSchema="JSON.parse(title.formSchema)"
          :playerName="this.playerName"
        />
      </div>
    </div>
  </div>
</template>

<style>
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
  }
  .gallery {
    display: table;
    width: 100%;
    
  }
  .placement {
    text-align: center;
  }
</style>
