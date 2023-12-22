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
    
    // 카드 뒤집기
    public void openCard()
    {
        if (gameManager.I.cardCounter > 1) return;

        gameManager.I.cardCounter++;

        // 카드 뒤집는 효과음 재생
        audioSource.PlayOneShot(card_flip);

        // 카드 뒤집는 애니메이션 활성화
        anim.SetBool("isOpen", true);

        // 카드 뒤집기
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);

        if(gameManager.I.firstCard == null)
        {
            // firstCard가 null이면 firstCard로 넣어버리기
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
