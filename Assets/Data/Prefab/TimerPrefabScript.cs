using System.Collections;
using UnityEngine;

public class TimerPrefabScript : MonoBehaviour {

    Coroutine coroutine = null;

    // 0 - не установлен
    // 1 - завершен
    // 2 - в процессе
    [HideInInspector]
    public int status = 0;

    IEnumerator WaitTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SetStatus(1);
        coroutine = null;
    }

    public void SetTimer(float time)
    {
        SetStatus(2);
        coroutine = StartCoroutine("WaitTimer", time);
    }

    public void ResetTimer()
    {
        SetStatus(0);
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    public void SetStatus(int _status)
    {
        status = _status;
    }
}
