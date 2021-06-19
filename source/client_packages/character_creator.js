let localPlayer = mp.players.local;
let browser = mp.browsers.new("package://IMRP/cef/character_creator.html");
browser.active = false;

charData = {
    'head': [
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 },
        { index: 255, opacity: 1.0, color: 0 }
    ],
    'gender': 0,
    'hair': { style: 0, color: 0, highlights: 0 },
    'skinColor': 0,
    'eyeColor': 0,
    'face': {
        mother: 0,
        father: 0,
        character: 0,
        resemblance: 0.0,
        data: [
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        ]
    },
    'clothes': {
        top: { ctype: 'T-Shirt', cid: 0 },
        pant: { ctype: 'Jean', cid: 0 },
        shoes: { ctype: 'Sneaker', cid: 0 }
    }
};

getCompentId = (type) => {

    if (type == 'top') return 11;
    if (type == 'pant') return 4;
    if (type == 'shoes') return 6;
};

var nakedClothes = {
    // [face, Mask, Hair, Torso, Legs, Bag, Feet, Accessories, Undershirt, Body Armor, Decals, Tops]
    0: [0, 0, 0, 15, 61, 0, 34, 0, 15, 0, 0, 15], // Male Clothes = 0
    1: [0, 0, 0, 15, 15, 0, 35, 0, 2, 0, 0, 15] // Female Clothes = 1
};

creator_view = mp.cameras.new('creator_view', new mp.Vector3(-903.272278320312, -364.6798095703125, 113.82000732421875), new mp.Vector3(-10,0,25.2321533203125), 40);

mp.events.add(
    {
        "initalizeCharacterCreator": () =>{

            mp.game.gameplay.setFadeOutAfterDeath(false);
            mp.gui.cursor.visible = true;
            mp.gui.chat.show(false);
            mp.gui.chat.activate(false);
            mp.game.ui.displayRadar(false);

            creator_view.setActive(true);
            mp.game.cam.renderScriptCams(true, false, 0, true, false);
            
           // const player = mp.players.local;
            //player.setAlpha(0);
            localPlayer.freezePosition(true);
            localPlayer.model = 1885233650;
            localPlayer.setComponentVariation(8, 15, 0, 0);
            localPlayer.setComponentVariation(3, 15, 0, 0);
            localPlayer.setComponentVariation(11, 15, 0, 0);

            localPlayer.setComponentVariation(3, nakedClothes[0][3],0,0);
            localPlayer.setComponentVariation(4, nakedClothes[0][4],0,0);
            localPlayer.setComponentVariation(6, nakedClothes[0][6], 0, 0);
            localPlayer.setComponentVariation(11, nakedClothes[0][11], 0, 0);
            localPlayer.setComponentVariation(8, nakedClothes[0][8], 0, 0);

            if(!browser.active) browser.active = true;
            mp.discord.update('Playing IMPACT ROLEPLAY', 'Creating a Character');
        },
        'creator_FaceCam':(value) =>{
            if(value)
            {
                creator_view.setCoord(-903.61981250117188,-363.8805847167969,113.85001373291016);
            }
            else
            {
                creator_view.setCoord(-903.272278320312, -364.6798095703125, 113.82000732421875);
            }
        },
        'creator_ChangeGender':(value) => {
            charData.gender = value;
            if (value == 0 && localPlayer.model != 1885233650) {
                localPlayer.model =  1885233650;
               //client.browser.getBrowser('character').execute('$("#beard").toggle();');
                localPlayer.setComponentVariation(3, 15, 0, 0);
                localPlayer.setComponentVariation(11, 15, 0, 0);
                localPlayer.setComponentVariation(3, nakedClothes[0][3],0,0);
                localPlayer.setComponentVariation(4, nakedClothes[0][4],0,0);
                localPlayer.setComponentVariation(6, nakedClothes[0][6], 0, 0);
                localPlayer.setComponentVariation(11, nakedClothes[0][11], 0, 0);
                localPlayer.setComponentVariation(8, nakedClothes[0][8], 0, 0);
            } else if (value != 0 && localPlayer.model != -1667301416) {
                //client.browser.getBrowser('character').execute('$("#beard").toggle();');
                localPlayer.model = -1667301416;
                 localPlayer.setComponentVariation(3, nakedClothes[1][3],0,0);
                 localPlayer.setComponentVariation(4, nakedClothes[1][4],0,0);
                 localPlayer.setComponentVariation(6, nakedClothes[1][6], 0, 0);
                 localPlayer.setComponentVariation(11, nakedClothes[1][11], 0, 0);
                 localPlayer.setComponentVariation(8, nakedClothes[1][8], 0, 0);
                //charData.head[1].index = 255; // Disable Beard is player choose Female
                //charData.head[11].index = 255; // Disable Chest Hair
            }
            
            localPlayer.setHeadBlendData(charData.face.mother, charData.face.father, charData.face.character, charData.skinColor, charData.skinColor, charData.skinColor, (charData.face.resemblance < 0.0) ? Math.abs(charData.face.resemblance) : 0.0, (charData.face.resemblance > 0.0) ? charData.face.resemblance : 0.0, 0, false);
            
            localPlayer.setComponentVariation(2, charData.hair.style, 0, 0);
            localPlayer.setHairColor(charData.hair.color, charData.hair.highlights);
        },
        'creator_changeParents': (father, mother, resemblance) => {
            charData.face.mother = mother;
            charData.face.father = father;
            charData.face.resemblance = resemblance;
            localPlayer.setHeadBlendData(charData.face.mother, charData.face.father, charData.face.character, charData.skinColor, charData.skinColor, charData.skinColor, (charData.face.resemblance < 0.0) ? Math.abs(charData.face.resemblance) : 0.0, (charData.face.resemblance > 0.0) ? charData.face.resemblance : 0.0, 0, false);
            mp.events.callRemote('writeToConsole', `${father} ${mother} ${resemblance}`);
        },
        'creator_changeHair': (style, color, highlight) => {
            if(charData.gender == 0 && style >= 23) {
                style++;
            }
            if(charData.gender != 0 && style >= 24) {
                style++;
            }
            charData.hair.style = style;
            charData.hair.color = color;
            charData.hair.highlights = highlight;
            localPlayer.setComponentVariation(2, style, 0, 0);
            localPlayer.setHairColor(color, highlight);
        },
        'creator_setHeadOverlayColor': (overlayID, color) => {
            let colorType = 0;
            if (1 == overlayID || 2 == overlayID || 10 == overlayID) colorType = 1;
            if (8 == overlayID || 5 == overlayID) colorType = 2;
            charData.head[overlayID].color = color;
            localPlayer.setHeadOverlayColor(overlayID, colorType, color, color);
        },
        'creator_setHeadOverlay': (overlayID, index, opacity) => {
            curZoom = overlayID;
            charData.head[overlayID].index = index;
            charData.head[overlayID].opacity = opacity;
            
            localPlayer.setHeadOverlay(overlayID, index, opacity, charData.head[overlayID].color, charData.head[overlayID].color);
            if (1 == overlayID || 2 == overlayID || 10 == overlayID || 8 == overlayID || 5 == overlayID) {
                mp.events.call('creator_setHeadOverlayColor', overlayID, charData.head[overlayID].color);
            }
        },
        'creator_changeSkinColor': (value) => {
            charData.skinColor = value;
            localPlayer.setHeadBlendData(charData.face.mother, charData.face.father, charData.face.character, charData.skinColor, charData.skinColor, charData.skinColor, (charData.face.resemblance < 0.0) ? Math.abs(charData.face.resemblance) : 0.0, (charData.face.resemblance > 0.0) ? charData.face.resemblance : 0.0, 0, false);
        },
        'creator_setFaceFeature': (index, scale) => {
            charData.face.data[index] = scale;
            localPlayer.setFaceFeature(index, scale);
        },
        'transitionToSelector': (index, scale) => {
            browser.reload(true);
            creator_view.setActive(false);
            browser.active = false;
            mp.events.call('initalizeCharacterSelector');
        },
        'creator_Finish': (firstname, lastname, age) => {
            browser.active = false;
            mp.events.callRemote("finishcharacter", firstname, lastname, age, JSON.stringify(charData));
        } ,
        'creator_Destroy':() =>{
            browser.destroy();
        }
    }
)

/*mp.events.add('render', function() {
    let coord = creator_view.getCoord();
    mp.game.graphics.drawText(`Cam Pos: X: ${coord.x}  Y: ${coord.y}  z: ${coord.z}`, [0.5, 0.10], {
        font: 4,
        color: [255, 255, 255, 255],
        scale: [0.4, 0.4],
        outline: true
    });
});

mp.keys.bind(0x64, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x, coord.y-0.01, coord.z);
    renderText();
});

mp.keys.bind(0x66, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x, coord.y+0.01, coord.z);
    renderText();
});

mp.keys.bind(0x61, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x-0.01, coord.y, coord.z);
    renderText();
});

mp.keys.bind(0x63, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x+0.01, coord.y, coord.z);
    renderText();
});

mp.keys.bind(0x68, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x, coord.y, coord.z+0.01);
    renderText();
});

mp.keys.bind(0x62, true, function() {
    let coord = creator_view.getCoord();
    creator_view.setCoord(coord.x, coord.y, coord.z-0.01);
    renderText();
});*/

