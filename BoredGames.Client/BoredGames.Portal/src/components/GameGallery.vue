<script>
import GameTitleTile from './GameTitleTile.vue'

export default {
  components: {
    GameTitleTile
  },

  data() {
    return {
      titles: [
        {
          id: 123,
          name: "Rock Paper Scissors",
          thumbnailImageUrl: "https://i.ibb.co/9n2tfBr/2023-12-19-0us-Kleki.png"
        },
        {
          id: 234,
          name: "Heartstone",
          thumbnailImageUrl: "https://i.ibb.co/9n2tfBr/2023-12-19-0us-Kleki.png"
        },
        {
          id: 345,
          name: "Covjece ne ljuti se",
          thumbnailImageUrl: "https://i.ibb.co/9n2tfBr/2023-12-19-0us-Kleki.png"
        }
      ]
    }
  }
}
</script>

<template>
  <GameTitleTile
    v-for="title in titles"
    :id="title.id"
    :title="title.name"
    :iconUrl="title.thumbnailImageUrl"
  />

</template>
