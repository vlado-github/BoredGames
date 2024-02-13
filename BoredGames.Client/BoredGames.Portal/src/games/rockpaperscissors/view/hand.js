import * as PIXI from 'pixi.js';
import { Card } from './card';



export class Hand extends PIXI.Container{
    constructor(gameSettings, displaySettings, isOpponentHand) {
        super();

        this.isOpponentHand = isOpponentHand;
        this.gameSettings = gameSettings;
        this.displaySettings = displaySettings;
        this.handCards = gameSettings.cardDeck;
        this.#setup()
    }

    #setup() {
        const w = this.displaySettings.screenWidth/3;
        const h = this.displaySettings.screenHeight/3;

        if (this.isOpponentHand) {
            for (let i=0; i<this.handCards.length; i++) {
                let card = new Card(
                    this.gameSettings.gameId,
                    this.handCards[i],
                    this.isOpponentHand);
                card.setPosition(
                    this.displaySettings.screenWidth - 2*w + i*w - this.displaySettings.cardWidth,
                    this.displaySettings.screenHeight - 2*h - this.displaySettings.cardHeight);
                card.setScale(
                    this.displaySettings.cardWidth,
                    this.displaySettings.cardHeight,
                    this.displaySettings.screenWidth);
                this.addChild(card);
            }
        }
        else {
            for (let i=0; i<this.handCards.length; i++) {
                let card = new Card(
                    this.gameSettings.gameId,
                    this.handCards[i],
                    this.isOpponentHand);
                card.setPosition(
                    this.displaySettings.screenWidth - 2*w + i*w - this.displaySettings.cardWidth,
                    this.displaySettings.screenHeight - this.displaySettings.cardHeight);
                card.setScale(
                    this.displaySettings.cardWidth,
                    this.displaySettings.cardHeight,
                    this.displaySettings.screenWidth);
                this.addChild(card);
            }
        }
    }
}