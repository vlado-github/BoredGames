<script>
import MenuDialog from './MenuDialog.vue'

export default {
  name: "gameTitleTile",
  props: {
    titleId: Number,
    titleName: String,
    iconUrl: String,
    formSchema: Array,
    playerName: ''
  },

  components: {
    MenuDialog
  },

  methods: {
    async pickGame() {
      await this.$refs.menuDialog.show();
    }
  }
}
</script>

<template>
  <div class="title">
    <div>
        <img @click="pickGame" v-bind:id="titleId" width="250" height="250" v-bind:src="iconUrl" v-bind:alt="titleName"/>
        <span class="caption">{{ titleName }}</span>
    </div>
    <MenuDialog ref="menuDialog" 
      :titleId="this.titleId" 
      :formSchema="this.formSchema"
      :playerName="this.playerName" />
  </div>  
</template>

<style scoped>
  .title {
    display: table-cell;
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
