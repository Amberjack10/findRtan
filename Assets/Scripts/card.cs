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
        // 카드가 2장 뒤집혀 있는 동안 다른 카드들 클릭 막기
        if (gameManager.I.cardCounter > 1) return;

        //cardCounter : 현재 게임에 뒤집혀 있는 카드 개수
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

        // cardCounter를 0으로 만들어 초기화 시켜주기
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
