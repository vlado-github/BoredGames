import * as PIXI from 'pixi.js';
import { Hand } from './hand';

export class Table extends PIXI.Container {
    constructor() { 
        super();
    }

    setup(gameSettings) {
        // set opponent's hand
        this.addChild(new Hand(gameSettings, true));

        // set player's hand
        this.addChild(new Hand(gameSettings, false));
    }
}