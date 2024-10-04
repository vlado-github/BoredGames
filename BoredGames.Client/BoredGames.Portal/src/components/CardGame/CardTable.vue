<script>
import PlayerHand from './PlayerHand.vue'
import GameEndDialog from './../GameEndDialog.vue'
import PlayerDialog from './../PlayerDialog.vue'
import InvitationDialog from './../InvitationDialog.vue'
import ScoreBoard from './ScoreBoard.vue'
import apiService from '@/api/api';
import LocalStorageKeys from '@/consts/localStorageKeys';
import GameStatusEnum from '@/consts/gameStatusEnum';

export default {
  name: 'cardTable',
  props: {
    gameInstanceId: ''
  },
  components: {
    PlayerHand,
    PlayerDialog,
    InvitationDialog,
    GameEndDialog,
    ScoreBoard
  },

  async mounted() {
    this.loadScore();
    this.startGameStatusCheck();

    this.currentPlayerId = localStorage.getItem(LocalStorageKeys.PlayerId);
    this.currentPlayerNickName = localStorage.getItem(LocalStorageKeys.PlayerNickName);

    if (!this.currentPlayerNickName) {
      await this.$refs.playerDialog.show();
    }
    else {
      this.joinGame();
      this.setTable();
    }
  },

  data() {
    return {
      gameStatusInterval: Number,
      gameStatus: 0, //AwaitingPlayers
      currentRound: 1,
      latestRound: 1,
      playersScores: [],
      cardDeck: [],
      currentPlayerId: '',
      currentPlayerNickName: ''
    };
  },

  methods: {
    async loadScore() {
      let score = await apiService.getGameScore(this.gameInstanceId);
      this.playersScores = score.playerScores;
    },

    async joinGame() {
      await apiService.joinGame({
          gameId: this.gameInstanceId,
          playerNickName: this.currentPlayerNickName
      });
    },

    async setTable() {
      let response = await apiService.getGameDefinition(this.gameInstanceId);
      this.cardDeck = response.assets["card_deck"];

      response = await apiService.getGameState(this.gameInstanceId);
      if (response.gameStatus == GameStatusEnum.AwaitingPlayers) {
        this.showInviteDialog();
      }
    },

    showInviteDialog() {
      this.$refs.invitationDialog.show();
    },

    async startGameStatusCheck() {
      this.gameStatusInterval = setInterval(async () => {
          await apiService.getGameState(this.gameInstanceId)
            .then(async (response) => {
              this.gameStatus = response.gameStatus;
              this.latestRound = response.roundNumber;
              

              if (this.latestRound > this.currentRound) { //Round completed
                this.currentRound = this.latestRound;
                this.isRoundCompleted = true;
                this.loadScore();
              }

              if (this.gameStatus == 2) { //Game Finished
                clearInterval(this.gameStatusInterval);
                await this.$refs.gameEndDialog.show();
              }
            })
            .catch(err => {
              console.log(err)
            })
        }, 1000);
    },
  }
}
</script>

<template>
  <div class="cardtablewrapper">
    <button v-if="this.gameStatus == 0" @click="showInviteDialog" class="invite-dialog-button">Invite</button>
    <div class="cardtable">
      <PlayerHand
          :cards="this.cardDeck"
          :player="{ foe: true, joined: this.gameStatus != 0 }"
          :gameInstanceId="gameInstanceId"
          :gameStatus="gameStatus"
          :roundNumber="latestRound"
          :playerScore="playersScores.filter(x => x.playerId != this.currentPlayerId)[0]"
      />
      <ScoreBoard 
          :cardDeckSize="this.cardDeck.length"
          :playersScores="playersScores"
          :currentPlayerId="currentPlayerId"/>
      <PlayerHand
          :cards="this.cardDeck"
          :player="{ foe: false, joined: true}"
          :gameInstanceId="gameInstanceId"
          :gameStatus="gameStatus"
          :roundNumber="latestRound"
          :playerScore="playersScores.filter(x => x.playerId == this.currentPlayerId)[0]"
      />
    </div>
    <GameEndDialog ref="gameEndDialog" :gameInstanceId="gameInstanceId" />
    <PlayerDialog ref="playerDialog" :gameInstanceId="gameInstanceId" :onSaveCallback="this.setTable" />
    <InvitationDialog ref="invitationDialog" :gameInstanceId="gameInstanceId" />
  </div>
</template>

<style>
    .cardtablewrapper {
      width: 100%;
      height: 100%;
      padding-left: 10%;
      padding-right: 10%;
      background: #56a87e;
    }

    .cardtable {
      display: grid;
      grid-template-columns: 1fr 1fr 1fr;
      grid-template-rows: 1fr 15% 1fr;
      justify-content: center;
      align-content: center;
      width: 80%;
      height: 100%;
      row-gap: 2%;
      column-gap: 2%;
      padding-top: 2%;
      padding-bottom: 2%;
      justify-content: center;
      align-items: center;
    }

    .invite-dialog-button {
      pointer-events:visible;
      margin: 15px;
      padding: 15px;
      background-color: yellow;
      color: black;
      position: absolute;
      top: 0;
      right: 0;
    }
</style>
