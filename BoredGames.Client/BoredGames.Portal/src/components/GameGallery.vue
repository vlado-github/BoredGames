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
  <div class="background">
    <h2 class="header">Bored Games</h2>
    <div class="gallery">
      <GameTitleTile
        v-for="title in titles"
        :titleId="title.id"
        :titleName="title.name"
        :iconUrl="title.thumbnailImageUrl"
        :formSchema="JSON.parse(title.formSchema)"
      />
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
  }
  .header {
    color: whitesmoke;
    text-align: center;
  }
  .gallery {
    display: table-row;
  }
</style>
