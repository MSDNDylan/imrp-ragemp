const timerBarLib = require("./IMRP/progressbar/progressbar");

const useSpeedo = true;
const updateInterval = 500; // milliseconds, lower value = more accurate, at the cost of performance

const Natives = {
    IS_RADAR_HIDDEN: "0x157F93B036700462",
    IS_RADAR_ENABLED: "0xAF754F20EB5CD51A",
    SET_TEXT_OUTLINE: "0x2513DFB0FB8400FE"
};

let streetName = null;
let zoneName = null;
let isMetric = false;
let minimap = {};

let seatBelt = false;


let rewardBar;
let armorBar;
let healthBar;
let hungerBar;
let thirstBar;
let teamBar;
let characterbar;

let browser = mp.browsers.new("package://IMRP/cef/hud.html");
browser.active = false;

let playerInventory = [];

let currentTimeOutRunning = false;

mp.events.add(
    {
        "displayHud": (amount, health, armor, hunger, thirst, name) =>{
            health = health*0.01;
            armor = armor*0.01;
            hunger = hunger*0.01;
            thirst = thirst*0.01;

            walletBar = new timerBarLib.TimerBar("Wallet");
            walletBar.text = `$${amount}`;
            walletBar.textColor = [114, 204, 114, 255];

            /*armorBar = new timerBarLib.TimerBar("Armor", true);
            armorBar.progress = armor;
            armorBar.pbarFgColor = [235, 235, 235, 255];
            armorBar.pbarBgColor = [0, 0, 0, 255];

            healthBar = new timerBarLib.TimerBar("Health", true);
            healthBar.progress = health;
            healthBar.pbarFgColor = [224, 50, 50, 255];
            healthBar.pbarBgColor = [0, 0, 0, 255];*/

            hungerBar = new timerBarLib.TimerBar("Hunger", true);
            hungerBar.progress = hunger;
            hungerBar.pbarFgColor = [255, 94, 0, 255];
            hungerBar.pbarBgColor = [0, 0, 0, 255];

            thirstBar = new timerBarLib.TimerBar("Thirst", true);
            thirstBar.progress = thirst;
            thirstBar.pbarFgColor = [3, 152, 252, 255];
            thirstBar.pbarBgColor = [0, 0, 0, 255];

            characterbar = new timerBarLib.TimerBar("");
            characterbar.text = name;
        },
        "updateHudWallet": (amount) =>{
            walletBar.Text = `$${amount}`;
        },
       /* "updateHudHealth": (health) =>{
            health = health*0.01;
            healthBar.progress = health;
        },
        "updateHudArmor": (armor) =>{
            armor = armor*0.01;
            armorBar.progress = armor;
        },*/
        "updateHudHunger": (hunger) =>{
            hunger = hunger*0.01;
            hungerBar.progress = hunger;
        },
        "updateHudThirst": (thirst) =>{
            thirst = thirst*0.01;
            thirstBar.progress = thirst;
        },
        "updateHudCharacter": (name) =>{
            characterbar.text = name;
        },
        "updateHud":(amount, health, armor, hunger, thirst, name) =>{
            health = health*0.01;
            armor = armor*0.01;
            hunger = hunger*0.01;
            thirst = thirst*0.01;

            walletBar.Text = `$${amount}`;
            healthBar.progress = health;
            armorBar.progress = armor;
            hungerBar.progress = hunger;
            thirstBar.progress = thirst;
            characterbar.text = name;
        },
        "seatbeltBuckled" : (buckled) =>{
            seatBelt = buckled;
        },
        "refreshPlayerInventory" : (inventory) =>{
            playerInventory = inventory;
            browser.execute(`refreshPlayerInventory(${inventory});`);
        },
        "toggleInventory" : () =>{
            if (mp.gui.cursor.visible && !browser.active) return;
            if(currentTimeOutRunning && !browser.active)return;

            if(browser.active){
                browser.active = false;
                mp.gui.cursor.visible = false;
            }else{
                mp.events.callRemote('refreshPlayerInventory');
                //browser.reload(true);
                browser.active = true;
                mp.gui.cursor.visible = true;
            }

            currentTimeOutRunning = true;
            setTimeout(() => { currentTimeOutRunning = false }, 2000);
        }
    }
)

function getMinimapAnchor() {
    let sfX = 1.0 / 20.0;
    let sfY = 1.0 / 20.0;
    let safeZone = mp.game.graphics.getSafeZoneSize();
    let aspectRatio = mp.game.graphics.getScreenAspectRatio(false);
    let resolution = mp.game.graphics.getScreenActiveResolution(0, 0);
    let scaleX = 1.0 / resolution.x;
    let scaleY = 1.0 / resolution.y;

    let minimap = {
        width: scaleX * (resolution.x / (4 * aspectRatio)),
        height: scaleY * (resolution.y / 5.674),
        scaleX: scaleX,
        scaleY: scaleY,
        leftX: scaleX * (resolution.x * (sfX * (Math.abs(safeZone - 1.0) * 10))),
        bottomY: 1.0 - scaleY * (resolution.y * (sfY * (Math.abs(safeZone - 1.0) * 10))),
    };

    minimap.rightX = minimap.leftX + minimap.width;
    minimap.topY = minimap.bottomY - minimap.height;
    return minimap;
}

function drawText(text, drawXY, font, color, scale, alignRight = false) {
    mp.game.ui.setTextEntry("STRING");
    mp.game.ui.addTextComponentSubstringPlayerName(text);
    mp.game.ui.setTextFont(font);
    mp.game.ui.setTextScale(scale, scale);
    mp.game.ui.setTextColour(color[0], color[1], color[2], color[3]);
    mp.game.invoke(Natives.SET_TEXT_OUTLINE);

    if (alignRight) {
        mp.game.ui.setTextRightJustify(true);
        mp.game.ui.setTextWrap(0, drawXY[0]);
    }

    mp.game.ui.drawText(drawXY[0], drawXY[1]);
}

setInterval(() => {
    // only do stuff if radar is enabled and visible
    if (mp.game.invoke(Natives.IS_RADAR_ENABLED) && !mp.game.invoke(Natives.IS_RADAR_HIDDEN)) {
        isMetric = mp.game.gameplay.getProfileSetting(227) == 1;
        minimap = getMinimapAnchor();

        const position = mp.players.local.position;
        let getStreet = mp.game.pathfind.getStreetNameAtCoord(position.x, position.y, position.z, 0, 0);
        zoneName = mp.game.ui.getLabelText(mp.game.zone.getNameOfZone(position.x, position.y, position.z));
        streetName = mp.game.ui.getStreetNameFromHashKey(getStreet.streetName);
        if (getStreet.crossingRoad && getStreet.crossingRoad != getStreet.streetName) streetName += ` / ${mp.game.ui.getStreetNameFromHashKey(getStreet.crossingRoad)}`;
    } else {
        streetName = null;
        zoneName = null;
    }
}, updateInterval);

mp.events.add("render", () => {
    if (streetName && zoneName) {
        drawText(streetName, [minimap.rightX + 0.01, minimap.bottomY - 0.065], 4, [255, 255, 255, 255], 0.55);
        drawText(zoneName, [minimap.rightX + 0.01, minimap.bottomY - 0.035], 4, [255, 255, 255, 255], 0.5);

        let vehicle = mp.players.local.vehicle;
        if (vehicle && seatBelt)
        {
            drawText("Seatbelt: ON", [minimap.leftX , minimap.topY], 4, [0, 255, 0, 255], 0.30);
        }else if(vehicle && !seatBelt)
        {
            drawText("Seatbelt: OFF", [minimap.leftX , minimap.topY], 4, [255, 0, 0, 255], 0.30);
        }

        if (useSpeedo && vehicle) drawText(`${(vehicle.getSpeed() * (isMetric ? 3.6 : 2.236936)).toFixed(0)} ${(isMetric) ? "KM/H" : "MPH"}`, [minimap.rightX - 0.003, minimap.bottomY - 0.0485], 4, [255, 255, 255, 255], 0.45, true);
    }
});