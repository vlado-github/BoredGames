<script>
import apiService from '@/api/api';

export default {
  name: 'card',
  props: {
    cardType: '',
    selected: false,
    isAgainstPlayer: false
  },


  // data() {
  //   return {
  //     cardType: '',
  //     selected: false,
  //     iconUrl: `http://localhost:5173/assets/{{this.cardType}}.png`
  //   };
  // },

  // methods: {
  //   async onClick() {
  //       this.selected = true;
  //       await apiService.makeMove(new {
  //           gameId: '',
  //           actionType: this.cardType,
  //       });
  //   }
  // }
}
</script>

<template>
  <div v-bind:id="cardType" class="cardbox">
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

    .cardbox:hover {
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
