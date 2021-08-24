using Mek.Models;
using Mek.Remote;
using UnityEngine;

namespace Game.Scripts.Models
{
    public static class GameConfig
    {
        public static int ScoreOnFoundationPile = 10;
        public static int ScoreOnMainPile = 5;
        
        public static RemoteInt IntRemoteExample = new RemoteInt("intTest", 100);
        // public static RemoteJson<TestStruct> JsonRemoteExample = new RemoteJson<TestStruct>("jsonTest", JsonUtility.ToJson(new TestStruct(){Number =  150}));
        public static RemoteString StringRemoteExample = new RemoteString("stringTest", "string");
        public static RemoteLong LongRemoteExample = new RemoteLong("longTest", 100);
        public static RemoteFloat FloatRemoteExample = new RemoteFloat("floatTest", 2.5f);
    }
}
