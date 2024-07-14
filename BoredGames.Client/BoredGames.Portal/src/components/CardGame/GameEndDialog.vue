<script>
import apiService from '@/api/api';

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
    }
  },

  methods: {
    async show() {
        console.log('show dialog')
        this.showModal = true;
        this.winners = await apiService.getGameWinner(this.gameInstanceId);
        console.log('show dialog' + this.showModal + ' [] ' +JSON.stringify(this.winners))
    }
  }
}
</script>

<template>
  <transition name="fade" appear>
    <div class="modal-dialog-overlay" 
         v-if="showModal" 
         @click="showModal = false">
    </div>
  </transition>
  <transition name="pop" appear>
    <div class="modal-dialog" 
         role="dialog" 
         v-if="showModal"
         >
      <h1>Vue Transitions</h1>
      <p>The transition component in Vue can create wonderful animated entrances and exits.</p>
      <button @click="showModal = false" class="button">Close</button>
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
        cursor: pointer;
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