// using System;
// using UnityEngine;

// public class PlayerDamageHandler : IDamageable
// {
//     // readonly AudioPlayer _audioPlayer;
//     // readonly Settings _settings;
//     readonly PlayerController _player;

//     public PlayerDamageHandler(
//         PlayerController player,
//         Settings settings,
//         AudioPlayer audioPlayer)
//     {
//         _audioPlayer = audioPlayer;
//         _settings = settings;
//         _player = player;
//     }

//     public void TakeDamage(int amount, Vector3 hitDirection)
//     {
//         // _audioPlayer.Play(_settings.HitSound, _settings.HitSoundVolume);

//         _player.AddForce(-hitDirection * _settings.HitForce);
//         _player.TakeDamage(amount);
//     }

//     [Serializable]
//     public class Settings
//     {
//         public float HitForce = 10f;

//         // public AudioClip HitSound;
//         // public float HitSoundVolume = 1f;
//     }
// }
