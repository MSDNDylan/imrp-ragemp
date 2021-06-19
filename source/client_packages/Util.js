let localPlayer = mp.players.local;

mp.events.add(
    {
        "getGroundZ": () =>{
            const getGroundZ = mp.game.gameplay.getGroundZFor3dCoord(localPlayer.position.x, localPlayer.position.y, localPlayer.position.z, parseFloat(0), false);
            mp.events.callRemote('SendChatMessage', getGroundZ);
        },
        "getGroundZForMethod": () =>{
            const getGroundZ = mp.game.gameplay.getGroundZFor3dCoord(localPlayer.position.x, localPlayer.position.y, localPlayer.position.z, parseFloat(0), false);
            mp.events.callRemote('SendGroundZ', getGroundZ);
        },
        "createMarker": (marker) =>{

            const getGroundZ = mp.game.gameplay.getGroundZFor3dCoord(localPlayer.position.x, localPlayer.position.y, localPlayer.position.z, parseFloat(0), false);
            marker.Position.Z = getGroundZ;
            mp.events.call('CreateMarker', marker);
        },
        "setWayPoint": (gotox, gotoy) =>{
            mp.game.ui.setNewWaypoint(gotox, gotoy);
        }
    }
)