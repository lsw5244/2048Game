using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public GameObject[] n;

    int x, y;
    Vector3 firstPos, gap;
    bool wait, move;

    GameObject[,] square = new GameObject[4, 4];

    void Start()
    {
        Spawn();
        Spawn();
    }

    void Update()
    {
        // 뒤로가기(종료)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // 처음 터치 한 위치 저장  // 터치 한손으로 && 터치발생상태 == 터치시작
        if(Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))  // 확인
        {
            wait = true;
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        }
        // 움직인 부분에서 처음 터치한 부분을 -하여 첫번째 터치와 두번째 터치의 gap 저장
        if(Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            gap = (Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
            // Debug.Log(gap);

            if(gap.magnitude < 100)
            {
                return;
            }
            // gap을 -1 ~ 1 사이로 함
            gap.Normalize();
            // Debug.Log(gap);
            if(wait)
            {
                wait = false;
                // 위로 드레그
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    //Debug.Log("위");
                }
                // 아래
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    //Debug.Log("아래");
                }
                // 오른쪽으로 >
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    //Debug.Log("오른쪽");
                }
                // 왼쪽 <
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    //Debug.Log("왼쪽");
                    for(y = 0; y <= 3; y++)
                    {
                        for(x = 3; x >= 1; x--)
                        {
                            for(int i = 0; i <= x-1; i++)
                            {
                                MoveOrCombine(i + 1, y, i, y);
                            }
                        }
                    }
                }
                else return;
                // 터치 했을 때 움직임이 있었으면 새로운 숫자 스폰시킴
                if(move)
                {
                    Spawn();
                    move = false;
                }
            }
        }
    }
    // x1, y1 이동 전 좌표, x2, y2 이동 될 좌표
    void MoveOrCombine(int x1, int y1, int x2, int y2)
    {
        // 이동 할 좌표가 비어있고 현재 좌표가 비어있지 않으면(이동할 수 있으면)
        if(square[x2, y2] == null && square[x1, y1] != null)
        {
            //square[x1, y1].transform.position = Vector3.MoveTowards(square[x1, y1].transform.position,
            //   new Vector3(-1.8f + 1.2f * x2, -1.8f + 1.2f * y2, 0), 0.1f);
            move = true;
            square[x1, y1].GetComponent<Move>().Moveing(x2, y2);
            square[x2, y2] = square[x1, y1];
            square[x1, y1] = null;
        }
    }

    // 다시 하기
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Spawn()
    {
        while(true)
        {
            x = Random.Range(0, 4);
            y = Random.Range(0, 4);

            if(square[x, y] == null)
            {
                break;
            }
        }

        square[x, y] = Instantiate(Random.Range(0, 8) > 0 ? n[0] : n[1]  // 1/8로 n[2], 7/8로 n[1]생성하도록 함
            , new Vector3(-1.8f + 1.2f * x, -1.8f + 1.2f * y, 0),  // 위치
            Quaternion.identity);
        square[x, y].GetComponent<Animator>().SetTrigger("Spawn");

    }
}
