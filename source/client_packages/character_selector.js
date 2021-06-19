let localPlayer = mp.players.local;
let browser = mp.browsers.new("package://IMRP/cef/character_selector.html");
browser.active = false;

selector_view = mp.cameras.new('selector_view', new mp.Vector3(-903.272278320312, -364.6798095703125, 113.82000732421875), new mp.Vector3(-10,0,25.2321533203125), 40);

let Username = "Unknown";
var characters = [];

mp.events.add(
    {
        "initalizeCharacterSelector": (username) =>{
            if(username != null) Username = username;
            Username = Username.charAt(0).toUpperCase() + Username.substr(1).toLowerCase();
            
            mp.discord.update('Playing IMPACT ROLEPLAY', `Selecting Character`);
            mp.events.callRemote("getCharacters");
        },
        "transistionToCreator": () =>{
            localPlayer.setAlpha(255);
            browser.reload(true);
            browser.active = false;
            selector_view.setActive(false);
            mp.events.call('initalizeCharacterCreator');
        },
        "initalizeSelectorData": (data) =>{
            mp.game.gameplay.setFadeOutAfterDeath(false);
            mp.gui.cursor.visible = true;
            mp.gui.chat.show(false);
            mp.gui.chat.activate(false);
            mp.game.ui.displayRadar(false);

            selector_view.setActive(true);
            mp.game.cam.renderScriptCams(true, false, 0, true, false);
            
            
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
            browser.execute(`updateMessage("${Username}");`);
            if(data == "empty")
            {
                localPlayer.setAlpha(0);
            }
            else
            {
                browser.execute(`populateCharacterData(${data});`);
                characters = JSON.parse(data);
                localPlayer.setAlpha(255);
            }
        },
        'changeCharacterSelection': (selection) =>{
            selection = parseInt(selection);
            let charData = characters[selection].CustomizeData;
            
            if(charData.gender == 0)
            {
                localPlayer.model = 1885233650;
            }
            else
            {
                localPlayer.model = -1667301416;
            }

            localPlayer.setHeadBlendData(charData.face.mother, charData.face.father, charData.face.character, charData.skinColor, charData.skinColor, charData.skinColor, (charData.face.resemblance < 0.0) ? Math.abs(charData.face.resemblance) : 0.0, (charData.face.resemblance > 0.0) ? charData.face.resemblance : 0.0, 0, false);
            localPlayer.setComponentVariation(2, charData.hair.style, 0, 0);
            localPlayer.setHairColor(charData.hair.color, charData.hair.highlights);

            charData.head.forEach(h => localPlayer.setHeadOverlay(charData.head.indexOf(h), h.index, h.opacity, h.color, h.color));

            charData.head.forEach(h => {
                let colorType = 0;
                if(1 == charData.head.indexOf(h) || 2 == charData.head.indexOf(h) || 10 == charData.head.indexOf(h)) colorType =1;
                if(8 == charData.head.indexOf(h) || 5 == charData.head.indexOf(h)) colorType = 2;
                localPlayer.setHeadOverlayColor(charData.head.indexOf(h), colorType, h.color, h.color);
            });

            localPlayer.setHeadBlendData(charData.face.mother, charData.face.father, charData.face.character, charData.skinColor, charData.skinColor, charData.skinColor, (charData.face.resemblance < 0.0) ? Math.abs(charData.face.resemblance) : 0.0, (charData.face.resemblance > 0.0) ? charData.face.resemblance : 0.0, 0, false);

            charData.face.data.forEach(f => localPlayer.setFaceFeature(charData.face.data.indexOf(f), f));

        },
        'deleteCharacter': (selection) =>{
            mp.events.callRemote('deleteCharacter', selection);
        },
        'selectorFinished': (selected) =>{
            mp.gui.cursor.visible = false;
            browser.destroy();
        
            selector_view.setActive(false);
            selector_view.destroy();
        
            mp.game.cam.renderScriptCams(false, false, 0, true, false);
            mp.gui.chat.show(true);
            mp.gui.chat.activate(true);
            mp.game.ui.displayRadar(true);
        
            localPlayer.setAlpha(255);
            localPlayer.freezePosition(false);
            mp.events.call('creator_Destroy');
            mp.events.callRemote('spawnCharacter', selected);

            mp.discord.update('Playing IMPACT ROLEPLAY', `Playing as ${characters[selected].FirstName} ${characters[selected].LastName}`);
            localPlayer.setNameDebug(`${characters[selected].FirstName} ${characters[selected].LastName}`);
        }
    }
)

