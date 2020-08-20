using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public GameObject[] n;

    int x, y;
    Vector3 firstPos, gap;

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
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

        }
        // 움직인 부분에서 처음 터치한 부분을 -하여 첫번째 터치와 두번째 터치의 gap 저장
        if(Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            gap = (Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
            Debug.Log(gap);

            if(gap.magnitude < 100)
            {
                return;
            }
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
