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
        const widthMargin = this.displaySettings.cardWidth/3;
        const heightMargin = this.displaySettings.cardHeight/5;

        if (this.isOpponentHand) {
            for (let i=0; i<this.handCards.length; i++) {
                let card = new Card(
                    this.gameSettings.gameId,
                    this.handCards[i],
                    this.isOpponentHand);
                card.setPosition(
                    i*(this.displaySettings.cardWidth+2*widthMargin) + widthMargin,
                    heightMargin);
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
                    i*(this.displaySettings.cardWidth+2*widthMargin) + widthMargin,
                    this.displaySettings.screenHeight - this.displaySettings.cardHeight - heightMargin);
                card.setScale(
                    this.displaySettings.cardWidth,
                    this.displaySettings.cardHeight,
                    this.displaySettings.screenWidth);
                this.addChild(card);
            }
        }
    }
}