import * as PIXI from 'pixi.js';

export class MessageBox extends PIXI.Text {
    constructor(name, message, fontSize){
        super();

        this.name = name;
        this.text = message;
        this.fontSize = fontSize;
    }

    setup(displaySettings){
        this.x = displaySettings.screenWidth/2;
        this.y = displaySettings.screenHeight/2;
        this.font = displaySettings.font;
        this.style.fill = displaySettings.messageFillColor;
    }
}