import * as PIXI from 'pixi.js';
import { Card } from './card';

const imageWidth = 250;
const imageHeight = 250;

export class Hand extends PIXI.Container{
    constructor(gameSettings, isOpponentHand) {
        super();

        this.isOpponentHand = isOpponentHand;
        this.gameSettings = gameSettings;
        this.handCards = ['rock', 'paper', 'scissors'];
        this.#setup()
    }

    #setup() {
        const w = this.gameSettings.screenWidth/3;
        const h = this.gameSettings.screenHeight/3;

        if (this.isOpponentHand) {
            for (let i=0; i<this.handCards.length; i++) {
                let card = new Card(
                    this.gameSettings.gameId,
                    this.handCards[i],
                    this.isOpponentHand);
                card.setPosition(
                    this.gameSettings.screenWidth - 2*w + i*w - imageWidth,
                    this.gameSettings.screenHeight - 2*h - imageHeight);
                card.setScale(
                    imageWidth,
                    imageHeight,
                    this.gameSettings.screenWidth);
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
                    this.gameSettings.screenWidth - 2*w + i*w - imageWidth,
                    this.gameSettings.screenHeight - imageHeight);
                card.setScale(
                    imageWidth,
                    imageHeight,
                    this.gameSettings.screenWidth);
                this.addChild(card);
            }
        }
    }
}