using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public Enemy parentCharacter;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentCharacter.isDead)
        {
            parentCharacter.attackList.Clear();
            return;
        }
        StartCoroutine("OnAlarm");
        parentCharacter.attackList.Add(collision.transform);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (parentCharacter.isDead)
        {
            parentCharacter.attackList.Clear();
            return;
        }
        parentCharacter.attackList.Remove(collision.transform);
    }

    IEnumerator OnAlarm()
    {
        parentCharacter.alarmSign.SetActive(true);
        yield return new WaitForSeconds(parentCharacter.alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        parentCharacter.alarmSign.SetActive(false);
    }
}
