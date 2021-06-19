let browser = mp.browsers.new("package://IMRP/cef/login.html");
browser.active = false;


/*mp.events.add("onPlayerConnectedEx", onPlayerConnectedEx());
function onPlayerConnectedEx () {
    mp.game.gameplay.setFadeOutAfterDeath(false);
    mp.gui.cursor.visible = true;
   // mp.gui.chat.show(false);
    mp.gui.chat.activate(false);
    mp.game.ui.displayRadar(false);

    login_view = mp.cameras.new('login_view', new mp.Vector3(-920.387512207031, -373.6016845703125, 114.31189727783203), new mp.Vector3(0, 0, 212.277), 40);
    login_view.pointAtCoord(new mp.Vector3(-906.3339233398438,-378.8407897949219,112.94166564941406));
    login_view.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);
    
    const player = mp.players.local;
    player.setAlpha(0);
    if(!browser.active) browser.active = true;

    mp.discord.update('Playing IMPACT ROLEPLAY', `Joining Server.`);
}

mp.events.add("show_login_browser", showLoginBrowser());
function  showLoginBrowser () {
    if(!browser.active) browser.active = true;
}

mp.events.add("hide_login_browser", hideLoginBrowser());
function  hideLoginBrowser () {
    if(browser.active) browser.active = false;
}

mp.events.add("login_finished", loginFinished());
function  loginFinished () {
    mp.gui.cursor.visible = false;
    browser.destroy();

    login_view.setActive(false);
    login_view.destroy();

    mp.game.cam.renderScriptCams(false, false, 0, true, false);
    mp.gui.chat.show(true);
    mp.gui.chat.activate(true);
    mp.game.ui.displayRadar(true);

    const player = mp.players.local;
    player.setAlpha(255);
    player.freezePosition(false);
}

mp.events.add("login", login(username, password));
function  login (username,password) {
    mp.events.callRemote("login",username, password);
}

mp.events.add("register", register(username, email, password));
function  register (username, email, password) {
    mp.events.callRemote("register", username, email, password);
}*/

mp.events.add("onPlayerConnectedEx", () => {
    mp.game.gameplay.setFadeOutAfterDeath(false);
    mp.gui.cursor.visible = true;
   // mp.gui.chat.show(false);
    mp.gui.chat.activate(false);
    mp.game.ui.displayRadar(false);

    login_view = mp.cameras.new('login_view', new mp.Vector3(-920.387512207031, -373.6016845703125, 114.31189727783203), new mp.Vector3(0, 0, 212.277), 40);
    login_view.pointAtCoord(-906.3339233398438,-378.8407897949219,112.94166564941406);
    login_view.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);
    
    const player = mp.players.local;
    player.setAlpha(0);
    if(!browser.active) browser.active = true;

    mp.discord.update('Playing IMPACT ROLEPLAY', `Joining Server.`);
});

mp.events.add("show_login_browser", () => {
    if(!browser.active) browser.active = true;
});

mp.events.add("hide_login_browser", () => {
    if(browser.active) browser.active = false;
});

mp.events.add("login_finished", () => {
    mp.gui.cursor.visible = false;
    browser.destroy();

    login_view.setActive(false);
    login_view.destroy();

    mp.game.cam.renderScriptCams(false, false, 0, true, false);
    mp.gui.chat.show(true);
    mp.gui.chat.activate(true);
    mp.game.ui.displayRadar(true);

    const player = mp.players.local;
    player.setAlpha(255);
    player.freezePosition(false);
});

mp.events.add("login", (username, password) => {
    mp.events.callRemote("login",username, password);
});

mp.events.add("register", (username, email, password) => {
    mp.events.callRemote("register", username, email, password);
});