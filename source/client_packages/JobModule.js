let localPlayer = mp.players.local;

let miningPoints = {};

mp.events.add(
    {
        "miningJobData": (position, direction, rotation, color, dimension) =>{

            let pos = new mp.Vector3(position.X, position.Y, position.Z);
            let dir = new mp.Vector3(direction.X, direction.Y, direction.Z);
            let rot= new mp.Vector3(rotation.X, rotation.Y, rotation.Z);

            let mark = mp.markers.new(3, pos, 0.5);
            miningPoints.marker = mark;
        },
        "cleanupPickups": () =>{
            
            if(miningPoints.length <= 0) return;

            for(m = 0; m < Object.keys(miningPoints).length; m++) {
                if(miningPoints[m].marker == undefined) return;
                miningPoints[m].marker.Destroy();
            }

            miningPoints = {};
        }
    }
)