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
      const response = await apiService.getTitles();
      this.titles = response.titles;
    }
  }
}
</script>

<template>
    <div class="gallery">
      <div class="placement">
        <GameTitleTile
          v-for="title in titles"
          :key="title.id"
          :titleId="title.id"
          :titleName="title.name"
          :iconUrl="title.thumbnailImageUrl"
        />
      </div>
    </div>
</template>

<style>
  .gallery {
    display: table;
    width: 100%;
  }
  .placement {
    text-align: center;
  }
</style>
