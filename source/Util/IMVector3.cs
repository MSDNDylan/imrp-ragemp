using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP
{
    public class IMVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        public IMVector3(Vector3 vector3)
        {
            X = vector3.X;
            Y = vector3.Y;
            Z = vector3.Z;
        }
        public Vector3 GetVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
