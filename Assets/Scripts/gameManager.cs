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
    
    // gameManager �̱���
    private void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale= 1.0f;

        int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        // rtans ����Ʈ�� �����ϰ� �����ϱ�
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for(int i = 0; i < 16; i++)
        {
            // card�� cards�� �ڽ����� �ֱ�
            GameObject newCard = Instantiate(card);
            newCard.transform.parent = GameObject.Find("cards").transform;

            // ī�� X, Y��ǥ ��ġ�ϱ�
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            newCard.transform.position = new Vector3(x, y, 0);

            // ī�� �̹��� �ֱ�
            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �ð� �帣�� �ϱ�
        time -= Time.deltaTime;

        // 30�� �Ǹ� ���� ���߱�
        if(time <= 0.0f)
        {
            time = 0.0f;    // �ð� ���ҷ� 0���� �۾����� ���� ����!
            
            //////////////////////////////////////////////////////////////////
            // ���â ����
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
        counter++;      // �õ� Ƚ�� ����

        // ù ��°, �� ��° ī���� �̹��� �̸��� ������ ���ϱ�
        string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            // �� ī���� �̹��� �̸��� ���� ��� destroy
            firstCard.GetComponent<card>().destroyCard();
            secondCard.GetComponent<card>().destroyCard();

            // ī�� �� ���߸� ���� ���߱�
            int cardsLeft = GameObject.Find("cards").transform.childCount;
            if(cardsLeft == 2)
            {
                // ���â ����
                endPanel.gameObject.SetActive(true);
                count.text = counter.ToString();
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            // ī�� �ٽ� ������
            firstCard.GetComponent<card>().closeCard();
            secondCard.GetComponent<card>().closeCard();

            // ������ ������ �ð� ����!
            time -= 3.0f;
        }

        // �ٽ� null�� �ʱ�ȭ ���Ѽ� ī�� �� ���� ������ �� �ֵ��� ����� �ֱ�
        firstCard = null;
        secondCard = null;
    }
}
