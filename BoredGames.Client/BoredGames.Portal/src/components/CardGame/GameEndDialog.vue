<script>
import apiService from '@/api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: 'gameEndDialog',
  expose: ['show'],
  props: {
    gameInstanceId: ''
  },

  data() {
    return {     
        winners: [],
        showModal: false,
        gameScore: {},
        isCurrentUserWinner: false
    }
  },

  methods: {
    async show() {
        this.showModal = true;
        this.winners = await apiService.getGameWinner(this.gameInstanceId);
       
        const playerId = localStorage.getItem(LocalStorageKeys.PlayerId);
        this.isCurrentUserWinner = this.winners.some(x => x.id == playerId);
    },

    quit(event) {
      this.showModal = false;
      this.$router.push({ name: 'home' })
    },

    playAgain(event) {
      this.showModal = false;
      //todo: restart game on server
      router.push({ name: 'game', props: { gameInstanceId: this.gameInstanceId } })
    }
  }
}
</script>

<template>
  <transition name="fade" appear>
    <div class="modal-dialog-overlay" 
         v-if="showModal">
    </div>
  </transition>
  <transition name="pop" appear>
    <div class="modal-dialog" 
         role="dialog" 
         v-if="showModal"
         >
      <h1 v-if="this.isCurrentUserWinner" style="color: green;">Victory</h1>
      <h1 v-else style="color: orange;">Defeat</h1>
      <button @click="quit" class="button">Quit</button>
      <button @click="playAgain" class="button">Play Again</button>
    </div>
  </transition>
</template>

<style>
    .modal-dialog {
        display: inline-block;
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        margin: auto;
        text-align: center;
        align-content: center;
        align-items: center;
        width: fit-content;
        height: fit-content;
        max-width: 22em;
        max-height: 44em;
        padding: 2rem;
        border-radius: 1rem;
        box-shadow: 0 5px 5px rgba(0, 0, 0, 0.2);
        background: #FFF;
        z-index: 999;
        transform: none;
        pointer-events: auto;
    }
    .modal-dialog h1 {
        margin: 0 0 1rem;
    }

    .modal-dialog-overlay {
        content: '';
        position: absolute;
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        z-index: 998;
        background: #2c3e50;
        opacity: 0.6;
    }

    /* ---------------------------------- */
    .fade-enter-active,
    .fade-leave-active {
        transition: opacity .4s linear;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }

    .pop-enter-active,
    .pop-leave-active {
        transition: transform 0.4s cubic-bezier(0.5, 0, 0.5, 1), opacity 0.4s linear;
    }

    .pop-enter,
    .pop-leave-to {
        opacity: 0;
        transform: scale(0.3) translateY(-50%);
    }
</style>