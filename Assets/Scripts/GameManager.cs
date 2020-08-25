using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public GameObject[] n;
    public GameObject quit;

    int x, y, j, l;
    Vector3 firstPos, gap;
    bool wait, move, isGameOver;

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

        if(isGameOver)
        {
            return;
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
                    for(int x = 0; x <= 3; x++)
                    {
                        for(int y = 0; y <= 2; y++)
                        {
                            for(int i = 3; i >= y+1; i--)
                            {
                                MoveOrCombine(x, i - 1, x, i);
                            }
                        }
                    }
                }
                // 아래
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    //Debug.Log("아래");
                    for(int x = 0; x <= 3; x++)
                    {
                        for(int y = 3; y >= 1; y--)
                        {
                            for(int i = 0; i <= y-1; i++)
                            {
                                MoveOrCombine(x, i + 1, x, i);
                            }
                        }
                    }
                }
                // 오른쪽으로 >
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    //Debug.Log("오른쪽");
                    for(int y = 0; y <= 3; y++)
                    {
                        for(int x = 0; x <= 2; x++)
                        {
                            for(int i = 3; i >= x+1; i--)
                            {
                                MoveOrCombine(i - 1, y, i, y);
                            }
                        }
                    }
                }
                // 왼쪽 <
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    //Debug.Log("왼쪽");
                    for(int y = 0; y <= 3; y++)
                    {
                        for(int x = 3; x >= 1; x--)
                        {
                            for(int i = 0; i <= x-1; i++)
                            {
                                MoveOrCombine(i + 1, y, i, y);
                            }
                        }
                    }
                }
                else return;
                // 터치 했을 때 움직임이 있었으면 새로운 숫자 스폰시킴 (드레그의 모든 처리가 끝냈을 때)
                if(move)
                {
                    Spawn();
                    move = false;
                    // 빈 칸의 개수를 저장 할 변수
                    int k = 0;
                    // Combine테그 떼어주기
                    for(int i = 0; i <= 3; i++)
                    {
                        for(int j = 0; j <= 3; j++)
                        {
                            if(square[i, j] == null)
                            {
                                // 비어있는 칸 저장  // 칸이 가득 차면 k가 0이 됨
                                k++;
                                continue;
                            }
                            if(square[i, j].tag == "Combine")
                            {
                                square[i, j].tag = "Untagged";
                            }
                        }
                    }
                    // 비어있는 칸이 0개일때 게임오버 검사
                    if (k == 0)
                    {
                        // 인접한 타일에 같은 수가 있는 개수를 저장하는 변수
                        l = 0;
                        // 가로, 세로 같은 블럭이 있으면 l이 0이 됨 (게임오버)
                        // 오른쪽에 같은 블럭이 있는지 확인
                        for (int y = 0; y <= 3; y++)
                        {
                            for (int x = 0; x <= 2; x++)
                            {
                                if (square[x, y].name == square[x + 1, y].name)
                                {
                                    l++;
                                }
                            }
                        }
                        // 위쪽에 같은 블럭이 있는지 확인
                        for (int x = 0; x <= 3; x++)
                        {
                            for (int y = 0; y <= 2; y++)
                            {
                                if (square[x, y].name == square[x, y + 1].name)
                                {
                                    l++;
                                }
                            }
                        }
                        // 게임 오버
                        if (l == 0)
                        {
                            isGameOver = true;
                            quit.SetActive(true);
                            return;
                        }
                    }

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
            square[x1, y1].GetComponent<Move>().Moveing(x2, y2, false);
            square[x2, y2] = square[x1, y1];
            square[x1, y1] = null;
        }

        // 같은 수 일때 결합하기
        if (square[x1, y1] != null && square[x2, y2] != null && square[x1,y1].name == square[x2,y2].name
            && square[x1, y1].tag != "Combine" && square[x2, y2].tag != "Combine")
        {
            move = true;
            for(j = 0; j <= 16; j++)
            {
                if(square[x2, y2].name == n[j].name + "(Clone)")
                {
                    break;
                }
            }
            square[x1, y1].GetComponent<Move>().Moveing(x2, y2, true);
            Destroy(square[x2, y2]);
            square[x1, y1] = null;
            square[x2, y2] = Instantiate(n[j + 1], new Vector3(-1.8f + 1.2f * x2, -1.8f + 1.2f * y2, 0), Quaternion.identity);
            square[x2, y2].tag = "Combine";
            square[x2, y2].GetComponent<Animator>().SetTrigger("Combine");
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
