import * as PIXI from 'pixi.js';

export class MessageBox extends PIXI.Text {
    constructor(name, message, fontSize){
        super();

        this.name = name;
        this.text = message;
        this.fontSize = fontSize;
    }

    setup(displaySettings){
        this.x = this.parent.width/2;
        this.y = 0;
        this.anchor.set(0.5);
        this.font = displaySettings.font;
        this.style.fill = displaySettings.messageFillColor;
    }
}