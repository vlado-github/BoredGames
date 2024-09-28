<script>
import GameStatusEnum from '@/consts/gameStatusEnum';

export default {
  name: 'card',
  props: {
    cardDeckSize: 0,
    cardType: '',
    isAgainstPlayer: false,
    gameInstanceId: '',
    gameStatus: 0,
    isRoundCompleted: false,
    isSelected: false,
    hide: true,
    reset: false
  },
  watch: { 
    isRoundCompleted: function(newValue, oldValue) { 
      if (newValue && this.isSelected) {
        this.render = true;
        this.showType = true;
      } else if (oldValue == true && newValue == false) {
        this.render = true;
        this.showType = false;
      } else {
        this.render = false;
        this.showType = false;
      }
    },
    reset: function(newValue){
      if (newValue) {
        this.render = true;
        this.showType = !this.isAgainstPlayer;
      }
    }
  },

  computed: {
    cssProps() {
      return {
        '--deck-size': (this.cardDeckSize + 1)
      }
    }
  },

  data() {
    return {
      render: true,
      showType: !this.isAgainstPlayer
    }
  },

  methods: {
    async onClick() {
      if (this.gameStatus == GameStatusEnum.InPlay) {
        this.$emit('cardSelected', this.cardType);
      }
    }
  }
}
</script>

<template>
  <div class="cardplaceholder" 
    :style="cssProps"
    v-if="render && hide"
    v-bind:isAgainstPlayer="isAgainstPlayer"
    v-bind:isSelected="isSelected">
    <div class='cardframe' 
      v-bind:id="cardType" 
      v-bind:isAgainstPlayer="isAgainstPlayer"
      v-bind:isSelected="isSelected"
      @click="onClick">
      <div class='cardcontent'>
          <img class="img-responsive"
            v-if="showType === true"
            v-bind:src="`/assets/${cardType}.png`" 
            v-bind:alt="cardType"/>
          <img class="img-responsive"
            v-if="showType === false"
            v-bind:src="`/assets/backside.png`" 
            alt="card's backside"/>
      </div>
    </div>
  </div>
</template>

<style>
  .cardframe[isAgainstPlayer=false]:hover  {
    cursor: pointer;
  }

  .cardframe[isAgainstPlayer=true] {
    pointer-events: none;
  }

  .cardplaceholder {
    align-content: center;
  }

  .cardplaceholder[isSelected=true] {
    grid-column-start: 1;
    grid-column-end: var(--deck-size);
  }

  .cardframe {
    display: flex;
    padding:15px;
    height: auto;
    max-width: 150px;
    max-height: 200px;
    border-radius: 5%;
    border: 2px solid #41CEE2;
    background-color: #F1ECE8;
    margin-left: auto;
    margin-right: auto;
    aspect-ratio: 3/4;
  }

  .cardcontent {
    display: block;
    border-radius: 5%;
    border: 2px solid #41CEE2;
    background-color: #FFFFFF;
    align-content: center;
  }

  .img-responsive {
    width: 100%;
    height: auto;
  }
</style>
