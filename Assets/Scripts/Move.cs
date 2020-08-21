using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool move = false;
    int _x2, _y2;

    void Update()
    {
        if(move)
        {
            Moveing(_x2, _y2);
        }
    }

    public void Moveing(int x2, int y2)
    {
        move = true;
        _x2 = x2;
        _y2 = y2;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.8f + 1.2f * x2, -1.8f + 1.2f * y2, 0), 0.3f);
        // 목적지에 도착 하면 업데이트의 moving 작동 그만하도록 함
        if(transform.position == new Vector3(-1.8f + 1.2f * x2, -1.8f + 1.2f * y2, 0))
        {
            move = false;
        }
    }
}
