import * as PIXI from 'pixi.js';
import { ButtonContainer } from '@pixi/ui';

export class DialogBox extends PIXI.Container {
    constructor(name, message, buttonConfig){
        super();

        this.name = name;
        this.message = message;
        this.buttonConfig = buttonConfig;
        this.fontSize = 36;
    }

    setup(displaySettings){
        const messageBox = new PIXI.Graphics();
        this.addChild(messageBox);
        messageBox.lineStyle(2, 0xFEEB77, 1);
        messageBox.beginFill(0x650A5A);
        messageBox.drawRect(
            0, 
            0, 
            displaySettings.screenWidth/3, 
            displaySettings.screenHeight/3);
        messageBox.endFill();

        const messageText = new PIXI.Text();
        messageText.text = this.message;
        messageText.fontSize = this.fontSize;
        messageText.font = displaySettings.font;
        messageText.style.fill = displaySettings.textFillColor;
        messageBox.addChild(messageText);
        messageText.x = messageText.parent.width/2;
        messageText.y = messageText.parent.height/2;
        messageText.anchor.set(0.5);


        const btnWidth = 100;
        const btnHeight = 50;
        const btnRadius = 15;
        
        const buttonBox = new PIXI.Graphics();
        buttonBox.beginFill(0xFEEB77);
        buttonBox.drawRoundedRect(messageBox.width/2 - btnWidth/2, 
            messageBox.height - btnHeight - messageBox.height/10, 
            btnWidth, 
            btnHeight, 
            btnRadius);
        buttonBox.endFill();
        messageBox.addChild(buttonBox);

    //     const buttonText = new PIXI.Text();
    //     buttonText.text = this.buttonConfig[0].text;
    //     buttonText.fontSize = this.fontSize/3;
    //     buttonText.font = displaySettings.font;
    //     buttonText.style.fill = displaySettings.textFillColor;
    //    buttonText.interactive = true;
    //    buttonText.
    //     buttonBox.addChild(buttonText);
    //     buttonText.x = buttonText.parent.width/2;
    //     buttonText.y = buttonText.parent.height/2;
    //     buttonText.anchor.set(0.5);

    //    messageBox.addChild(buttonBox);

        this.x = this.parent.width/2 - messageBox.width/3;
        this.y = this.parent.height/2 - messageBox.height/2;
    }
}