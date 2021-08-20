namespace Mek.Models
{
    public class MekPlayerData
    {
        public static bool IsStatSet
        {
            get => PrefsManager.GetBool(PrefStats.IsStatSet);
            set => PrefsManager.SetBool(PrefStats.IsStatSet, value);
        }

        public static bool MusicOpened
        {
            get => PrefsManager.GetBool(PrefStats.MusicOpened);
            set => PrefsManager.SetBool(PrefStats.MusicOpened, value);
        }
    }
}
