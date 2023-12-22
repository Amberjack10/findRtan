using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor.PackageManager;

public class gameManager : MonoBehaviour
{
    public static gameManager I;

    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endPanel;

    public Text timeTxt;
    public Text count;

    public float time = 60.0f;
    public int counter = 0;
    public int cardCounter = 0;
    
    // gameManager 싱글톤
    private void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale= 1.0f;

        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        // rtans 리스트를 랜덤하게 정렬하기
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for(int i = 0; i < 16; i++)
        {
            // card를 cards의 자식으로 넣기
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            // 카드 X, Y좌표 배치하기
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            newCard.transform.position = new Vector3(x, y, 0);

            // 카드 이미지 넣기
            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 시간 흐르게 하기
        time -= Time.deltaTime;

        // 30초 되면 게임 멈추기
        if(time <= 0.0f)
        {
            time = 0.0f;    // 시간 감소로 0보다 작아지는 것을 방지!
            
            //////////////////////////////////////////////////////////////////
            // 결과창 띄우기
            endPanel.gameObject.SetActive(true);
            count.text = counter.ToString();
            Time.timeScale = 0.0f;
        }

        timeTxt.text = time.ToString("N2");

        if (cardCounter > 1) card.GetComponent<Button>().interactable = false;
        else card.GetComponent<Button>().interactable = true;
    }

    public void isMatched()
    {
        counter++;      // 시도 횟수 증가

        // 첫 번째, 두 번째 카드의 이미지 이름을 가져와 비교하기
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            // 두 카드의 이미지 이름이 같을 경우 destroy
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            // 카드 다 맞추면 게임 멈추기
            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if(cardsLeft == 2)
            {
                // 결과창 띄우기
                endPanel.gameObject.SetActive(true);
                count.text = counter.ToString();
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            // 카드 다시 뒤집기
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();

            // 실패할 때마다 시간 감소!
            time -= 3.0f;
        }

        // 다시 null로 초기화 시켜서 카드 두 장을 선택할 수 있도록 만들어 주기
        firstCard = null;
        secondCard = null;
    }
}
