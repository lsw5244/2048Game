using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool move = false;
    bool _combine;
    int _x2, _y2;

    void Update()
    {
        if(move)
        {
            Moveing(_x2, _y2, _combine);
        }
    }

    public void Moveing(int x2, int y2, bool combine)
    {
        move = true;
        _x2 = x2;
        _y2 = y2;
        _combine = combine;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.865f + 1.245f * x2, -1.45f + 1.24f * y2, 0), 0.3f);
        // 목적지에 도착 하면 업데이트의 moving 작동 그만하도록 함
        if(transform.position == new Vector3(-1.865f + 1.245f * x2, -1.45f + 1.24f * y2, 0))
        {
            move = false;
            if(combine)
            {
                _combine = false;
                Destroy(this.gameObject);
            }
        }
    }
}
