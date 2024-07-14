<script>

export default {
  name: 'card',
  props: {
    cardType: '',
    isAgainstPlayer: false,
    gameInstanceId: ''
  },

  methods: {
    async onClick() {
      this.$emit('cardSelected', this.cardType);
    }
  }
}
</script>

<template>
  <div v-bind:id="cardType" v-bind:isAgainstPlayer="isAgainstPlayer" class="cardbox" @click="onClick">
    <div class='cardframe'>
      <div class='cardcontent'>
          <img  width="100" height="100" 
            v-if="isAgainstPlayer === false"
            v-bind:src="`/assets/${cardType}.png`" 
            v-bind:alt="cardType"/>
      </div>
    </div>
  </div>

</template>

<style>
    .cardbox {
      display: table-cell;
    }

    .cardbox[isAgainstPlayer=false]:hover  {
      cursor: pointer;
    }

    .cardframe {
      display: flex;
      margin: 15px;
      padding:15px;
      width: 150px;
      height: 200px;
      border-radius: 5%;
      border: 2px solid #41CEE2;
      background-color: #F1ECE8;
    }

    .cardcontent {
      display: flex;
      margin: 2%;
      width: 98%;
      border-radius: 5%;
      border: 2px solid #41CEE2;
      background-color: #FFFFFF;
      align-items: center;
      justify-content: center;
    }
</style>
