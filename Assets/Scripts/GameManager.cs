using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public GameObject[] n;

    int x, y;

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
