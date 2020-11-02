using UnityEngine;

namespace Planetarity
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager i;

        public enum Sound
        {
            ButtonHover, ButtonClick, MosquitoLaunch, WaspLaunch, CowLaunch, MosquitoExplosion, WaspExplosion, CowExplosion, PlanetExplosion
        }
        public enum Music
        {
            GameTheme
        }

        public float lowPitch = .95f;
        public float highPitch = 1.05f;

        public AudioSource source;

        public AudioClip buttonHover;
        public AudioClip buttonClick;

        public AudioClip[] mosquitoLaunch;
        public AudioClip[] waspLaunch;
        public AudioClip[] cowLaunch;

        public AudioClip[] mosquitoExplosion;
        public AudioClip[] waspExplosion;
        public AudioClip[] cowExplosion;

        public AudioClip planetExplosion;


        private void Awake()
        {
            if (i == null)
            {
                i = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public static AudioClip Clip(Sound sound)
        {
            switch (sound)
            {
                case Sound.ButtonHover: return SoundManager.i.buttonHover;
                case Sound.ButtonClick: return SoundManager.i.buttonClick;

                case Sound.MosquitoLaunch:
                    var mosquitoLaunchIndex = Random.Range(0, SoundManager.i.mosquitoLaunch.Length);
                    return SoundManager.i.mosquitoLaunch[mosquitoLaunchIndex];

                case Sound.WaspLaunch:
                    var waspLaunchIndex = Random.Range(0, SoundManager.i.waspLaunch.Length);
                    return SoundManager.i.waspLaunch[waspLaunchIndex];

                case Sound.CowLaunch:
                    var cowLaunchIndex = Random.Range(0, SoundManager.i.cowLaunch.Length);
                    return SoundManager.i.cowLaunch[cowLaunchIndex];

                case Sound.MosquitoExplosion:
                    var mosquitoExplosionIndex = Random.Range(0, SoundManager.i.mosquitoExplosion.Length);
                    return SoundManager.i.mosquitoExplosion[mosquitoExplosionIndex];

                case Sound.WaspExplosion:
                    var waspExplosionIndex = Random.Range(0, SoundManager.i.waspExplosion.Length);
                    return SoundManager.i.waspExplosion[waspExplosionIndex];

                case Sound.CowExplosion:
                    var cowExplosionIndex = Random.Range(0, SoundManager.i.cowExplosion.Length);
                    return SoundManager.i.cowExplosion[cowExplosionIndex];

                case Sound.PlanetExplosion: return SoundManager.i.planetExplosion;

                default: return SoundManager.i.buttonClick;
            }
        }

        public static void Play(Sound sound, float volume = 1f)
        {
            SoundManager.i.source.PlayOneShot(Clip(sound), volume);
        }

        public static void LaunchRocket(string name, float volume = 1f)
        {
            switch (name)
            {
                case "Mosquito":
                    Play(Sound.MosquitoLaunch, volume);
                    break;
                case "Wasp":
                    Play(Sound.WaspLaunch, volume);
                    break;
                case "Cow":
                    Play(Sound.CowLaunch, volume);
                    break;
            }
        }

        public static void ExplodeRocket(string name, float volume = 1f)
        {
            switch (name)
            {
                case "Mosquito":
                    Play(Sound.MosquitoExplosion, volume);
                    break;
                case "Wasp":
                    Play(Sound.WaspExplosion, volume);
                    break;
                case "Cow":
                    Play(Sound.CowExplosion, volume);
                    break;
            }
        }

        public static void ExplodePlanet(float volume = 1f)
        {
            Play(Sound.PlanetExplosion, volume);
        }

        public static void PlayRandomPitch(Sound sound, float volume = 1f)
        {
            float randomPitch = Random.Range(SoundManager.i.lowPitch, SoundManager.i.highPitch);
            var source = SoundManager.i.source;
            source.pitch = randomPitch;
            source.clip = SoundManager.Clip(sound);
            source.volume = volume;
            source.Play();
        }
    }
}
