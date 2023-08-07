using UnityEngine;

public class MicAudioPlayer {
    public const string VOLUME_KEY = "VOLUME";
    
    readonly Camera _camera;
    private GameSettingsInstaller.AudioMicSettings _audioMicSettings;
    private bool _isOn;

    public MicAudioPlayer(GameSettingsInstaller.AudioMicSettings settings) {
        _camera = Camera.main;
        _audioMicSettings = settings;
        _isOn = _audioMicSettings.isOn;
        if (PlayerPrefs.HasKey(VOLUME_KEY)) {
            _isOn = PlayerPrefs.GetInt(VOLUME_KEY) > 0;
        }
    }

    public void Play(AudioClip clip) {
        Play(clip, 1);
    }

    public void Play(AudioClip clip, float volume) {
        if (_isOn) {
            _camera.GetComponent<AudioSource>().PlayOneShot(clip, volume);
        }
    }

    public void DoRotate() {
        // Play(_audioMicSettings.startMove, 1f);
    }

    public void DoMove() {
        // Play(_audioMicSettings.startMove, 1f);
    }

    public void DoComplete() {
        // Play(_audioMicSettings.levelComplete, 1f);
    }

    public bool IsOn {
        get => _isOn;
        set {
            _isOn = value;
            PlayerPrefs.SetInt(VOLUME_KEY, value ? 1 : 0);
        }
    }
}