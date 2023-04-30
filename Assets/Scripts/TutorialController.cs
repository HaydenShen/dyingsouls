using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    bool moveLeft, moveRight, jump, shield, attack;
    public Text moveText, jumpShieldText, attackText, endTutorialText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            moveText.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveLeft = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveRight = true;
        }
        if (moveLeft && moveRight)
        {
            jumpShieldText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.W))
            {
                jump = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                shield = true;
            }

            if (jump && shield)
            {
                attackText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.J))
                {
                    attack = true;
                }

                if (attack)
                {
                    endTutorialText.gameObject.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SceneManager.LoadScene("main");
                    }
                }
            }
        }
    }
}
