import * as PIXI from 'pixi.js';
import apiService from '@/api/api';

const spritesheet = await PIXI.Assets.load(`${import.meta.env.VITE_BASE_URL}/assets/spritesheet.json`);

export class Card extends PIXI.Sprite{
    constructor(gameId, cardType, isFolded) {
        if(isFolded) {
            super(spritesheet.textures['backside.png']);
        } else {
            super(spritesheet.textures[`${cardType}.png`]);
        }

        this.gameId = gameId;
        this.cardType = cardType;

        this.eventMode = 'static';
        this.cursor = 'pointer';
        this.on('pointerdown', this.#onClick);
    }

    async setPosition(x, y) {
        this.x = x;
        this.y = y;
    }

    async setScale(width, height, screenWidth) {
        this.width = width;
        this.height = height;
        this.screenWidth = screenWidth;
    }

    async #onClick(event) {
        this.x = this.parent.width/2;
        this.anchor.x = 0.5;
        const unselectedCards = this.parent.children.filter(x => x.cardType != this.cardType);
        unselectedCards.forEach(card => {
            card.visible = false;
        });
        await this.#makeMove(this.cardType);
    }

    async #makeMove(move) {
        return await apiService.makeMove({
          gameId: this.gameId, 
          actionType: move 
        });
    }
}