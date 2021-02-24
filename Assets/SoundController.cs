using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static void PlaySound(AudioSource sound) {
        sound.Play();
    }
}
