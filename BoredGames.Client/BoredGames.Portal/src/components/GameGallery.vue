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
      <GameTitleTile
        v-for="title in titles"
        :key="title.id"
        :titleId="title.id"
        :titleName="title.name"
        :titleAlias="title.alias"
        :iconUrl="title.thumbnailImageUrl"
      />
    </div>
</template>

<style>
  .gallery {
    display: grid;
    width: 100%;
    row-gap: 50px;
    grid-template-columns: repeat(auto-fit, minmax(min(100%/3, max(64px, 100%/3)), 1fr));
  }
</style>
