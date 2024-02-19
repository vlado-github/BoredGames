<script>
import GameTitleTile from './GameTitleTile.vue'
import apiService from '../api/api';

export default {
  name: 'gameGallery',
  components: {
    GameTitleTile
  },

  data() {
    return {
      titles: [],
    };
  },
  mounted() {
    this.fetchTitles();
  },
  methods: {
    async fetchTitles() {
      this.titles = await apiService.getTitles();
    }
  }
}
</script>

<template>
  <div class="container mt-5 gallery">
    <h2 class="header">Bored Games</h2>
    
    <div class="row">
      <GameTitleTile
        v-for="title in titles"
        :id="title.id"
        :title="title.name"
        :iconUrl="title.thumbnailImageUrl"
      />
    </div>
  </div>

</template>

<style>
  .header {
    color: whitesmoke;
    text-align: center;
  }
  .gallery {
    background-color: #1b2939;
  }
</style>
