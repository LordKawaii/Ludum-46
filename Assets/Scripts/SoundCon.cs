using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCon : MonoBehaviour
{
    public AudioClip explosion1;
    public AudioClip explosion2;
    public AudioClip explosion3;
   
    public AudioClip engAlarm;
    public AudioClip secAlarm;

    public AudioClip powerDown;

    public AudioSource source1;
    public AudioSource source2;
    public void PlayExplosion()
    {
        int expNum = Random.Range(1, 4);
        if(expNum == 1)
        {
            PlaySound(explosion1, 1f);
        }
        else if (expNum == 2)
        {
            PlaySound(explosion2, 1f);
        }
        else 
        {
            PlaySound(explosion3, 1f);
            //if (!source1.isPlaying)
            //{
            //    source1.clip = explosion3;
            //    source1.Play();
            //}
            //else
            //{
            //    source2.clip = explosion3;
            //    source2.Play();
            //}
        }
    }

    public void PlayEngAlarm()
    {
        PlaySound(engAlarm, .8f);
        //if (!source1.isPlaying)
        //{
        //    source1.clip = engAlarm;
        //    source1.volume = .8f;
        //    source1.Play();
        //}
        //else
        //{
        //    source2.clip = engAlarm;
        //    source2.volume = .8f;
        //    source2.Play();
        //}
    }

    public void PlaySecAlarm()
    {
        PlaySound(secAlarm, .8f);
        //if (!source1.isPlaying)
        //{
        //    source1.clip = secAlarm;
        //    source1.volume = .8f;
        //    source1.Play();
        //}
        //else
        //{
        //    source2.clip = secAlarm;
        //    source2.volume = .8f;
        //    source2.Play();
        //}
    }

    public void PlayPowerDown()
    {
        PlaySound(powerDown, 1);
    }

    void PlaySound(AudioClip sound, float vol)
    {
        int speaker = Random.Range(1, 3);
        if (speaker == 1)
        {
            if (!source1.isPlaying)
            {
                source1.clip = sound;
                source1.volume = vol;
                source1.Play();
            }
            else
            {
                source2.clip = sound;
                source2.volume = vol;
                source2.Play();
            }
        }
        else
        {
            if (!source2.isPlaying)
            {
                source2.clip = sound;
                source2.volume = vol;
                source2.Play();
            }
            else
            {
                source1.clip = sound;
                source1.volume = vol;
                source1.Play();
            }
        }
    }
}
