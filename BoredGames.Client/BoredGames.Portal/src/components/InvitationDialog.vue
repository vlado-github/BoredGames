<script>
import LocalStorageKeys from '@/consts/localStorageKeys';
import { useToast, POSITION } from "vue-toastification";

export default {
  name: 'invitationDialog',
  expose: ['show'],
  props: {
    gameInstanceId: ''
  },

  data() {
    return {     
        showModal: false,
        inviteLinkUrl: ''
    }
  },

  async mounted() {
    this.inviteLinkUrl = `${import.meta.env.VITE_BASE_URL}/game?gameInstanceId=${this.gameInstanceId}`;
  },

  methods: {
    show() {
      this.showModal = true;
    },

    quit(event) {
      this.showModal = false;
      this.$router.push({ name: 'home' })
    },

    async copyLink(event) {
      this.showModal = false;
      this.$refs.invitelink.focus();
      await navigator.clipboard.writeText(this.inviteLinkUrl);
      this.showToast("Invite link copied.");
      this.showModal = false;
    },

    showToast(message) {          
      const toast = useToast();   
      toast.success(message, {
        timeout: 2000,
        position: POSITION.BOTTOM_LEFT,
      });        
    },
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
    
      <label>Copy invite link and send it to a friend.</label>
      <input 
           v-on:focus="$event.target.select()" 
           ref="invitelink" 
           readonly 
           :value="inviteLinkUrl"/>
      <button @click="copyLink" class="modal-dialog-button">Copy</button>
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