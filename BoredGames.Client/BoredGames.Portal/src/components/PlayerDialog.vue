<script>
import LocalStorageKeys from '@/consts/localStorageKeys';
import apiService from '@/api/api';
import InvitationDialog from './InvitationDialog.vue';
import GameStatusEnum from '@/consts/gameStatusEnum';

export default {
  name: 'playerDialog',
  expose: ['show'],
  props: {
    gameInstanceId: ''
  },

  components: {
    InvitationDialog
  },

  data() {
    return {     
        showModal: false,
        playerName: '',
        playerJoined: false,
        onSaveCallback: {}
    }
  },

  methods: {
    show() {
      this.showModal = true;
      this.playerName = localStorage.getItem(LocalStorageKeys.PlayerNickName);
    },

    quit(event) {
      this.showModal = false;
      this.$router.push({ name: 'home' })
    },

    async save(event) {
      this.showModal = false;
      localStorage.setItem(LocalStorageKeys.PlayerNickName, this.playerName);
      apiService.joinGame({
        gameId: this.gameInstanceId,
        playerNickName: this.playerName
      });

      let response = await apiService.getGameState(this.gameInstanceId);
      if (response.gameStatus == GameStatusEnum.AwaitingPlayers) {
        this.$refs.invitationDialog.show();
      }
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
        <div>
            <input v-model="this.playerName" placeholder="Enter player name..." />
            <div>
                <button @click="quit" class="modal-dialog-button">Quit</button>
                <button @click="save" class="modal-dialog-button">Save</button>
            </div>
        </div>
        <InvitationDialog ref="invitationDialog" 
          :gameInstanceId="this.gameInstanceId" />
        
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

    .modal-dialog-button {
      pointer-events:visible;
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