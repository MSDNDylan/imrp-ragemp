var keys = {
    F1: 0x70,
    F2: 0x71,
    F3: 0x72,
    F4: 0x73,
    F5: 0x74,
    E: 0x45,
    B: 0x42,
    I: 0x49,
    Y: 0x59
  };

mp.keys.bind(keys.F2, true, function() {
    if(mp.gui.cursor.visible) {
        mp.gui.cursor.visible = false;
    }
    else
    {
        mp.gui.cursor.visible = true;
    }
});


var interactionMenuActive = false;
mp.keys.bind(keys.B, true, function() {
    if (mp.gui.cursor.visible) return;
    if(!interactionMenuActive){
        mp.events.callRemote('openInteractionMenu');
        interactionMenuActive = true;
    }else{
        mp.events.call('destroyMenu');
        interactionMenuActive = false;
    }
});

mp.keys.bind(keys.I, true, function() {
    mp.events.call('toggleInventory');
});

mp.keys.bind(keys.Y, true, function() {
    mp.events.callRemote('PlayerPressedY');
});