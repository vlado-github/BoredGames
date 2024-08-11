<script>
import router from '@/router';
import apiService from '@/api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';

export default {
  name: 'customGameDialog',
  expose: ['show'],
  props: {
    titleId: '',
    playerName: '',
    formSchema: Array
  },

  data() {
    return {     
        showModal: false
    }
  },

  methods: {
    async show() {
        this.showModal = true;
    },

    quit(event) {
      this.showModal = false;
      this.$router.push({ name: 'home' })
    },

    async submit(data) { 
      const response = await apiService.createGame({
        gameTitle: this.titleId,
        numberOfPlayers: 2,
        requiredNumberOfConsecutiveWins: data.requiredNumberOfConsecutiveWins,
        numberOfRounds: 10,
        playerNickName: this.playerName
      });

      this.showModal = false;
      
      localStorage.setItem(LocalStorageKeys.GameId, response.gameId);
      
      router.push({ name: 'game', query: { gameInstanceId: response.gameId } });
    }
  }
}
</script>

<template>
  <transition name="fade" appear>
    <div class="modal-dialog-overlay" v-if="showModal">
    </div>
  </transition>
  <transition name="pop" appear>
    <div class="modal-dialog" 
         role="dialog" 
         v-if="showModal"
         >
      <FormKit type="form" @submit="submit" submit-label="Create">
        <FormKitSchema :schema="this.formSchema"></FormKitSchema>
        <FormKit type="button" @class="modal-dialog-button" @click="quit">Quit</FormKit>
      </FormKit>
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
        pointer-events: visible;
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

    button[type="button"] {
      pointer-events:visible;
      margin: 15px;
      padding: 15px;
      background-color: goldenrod;
      color: white;
    }

    button[type="submit"] {
      pointer-events:visible;
      margin: 15px;
      padding: 15px;
      background-color: green;
      color: white;
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