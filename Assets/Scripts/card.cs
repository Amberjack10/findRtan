using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class card : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip card_flip;

    public Animator anim;

    float timeSpan = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSpan - gameManager.I.time >= 5.0f)
        {
            timeSpan = 0.0f;
            gameManager.I.firstCard.GetComponent<card>().closeCard();
            
            gameManager.I.firstCard = null;
        }
    }
    
    // ī�� ������
    public void openCard()
    {
        if (gameManager.I.cardCounter > 1) return;

        gameManager.I.cardCounter++;

        // ī�� ������ ȿ���� ���
        audioSource.PlayOneShot(card_flip);

        // ī�� ������ �ִϸ��̼� Ȱ��ȭ
        anim.SetBool("isOpen", true);

        // ī�� ������
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if(gameManager.I.firstCard == null)
        {
            // firstCard�� null�̸� firstCard�� �־������
            gameManager.I.firstCard = gameObject;

            timeSpan = gameManager.I.time;
        }
        else
        {
            gameManager.I.secondCard= gameObject;
            gameManager.I.isMatched();
        }
    }

    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);

        gameManager.I.cardCounter = 0;
        timeSpan = 0.0f;
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 0.5f);
    }

    void closeCardInvoke()
    {
        anim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);

        gameManager.I.cardCounter = 0;
        timeSpan = 0.0f;
    }
}
