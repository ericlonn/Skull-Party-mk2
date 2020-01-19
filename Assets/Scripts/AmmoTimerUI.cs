using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTimerUI : MonoBehaviour
{
    public GameObject ammoCounter1, ammoCounter2, ammoCounter3;
    public GameObject timerBar;
    public GameObject barMask;
    public float barMaskStartingPosX, barMaskEndingPosX;
    Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Sign(_player.transform.localScale.x) == 1){
            transform.localScale = Vector2.one;

        } else {
            transform.localScale = new Vector2(-1,1);
        }
        
        if (_player.isPoweredUp)
        {
            if (timerBar.active == false) timerBar.SetActive(true);

            switch (transform.parent.GetComponent<PlayerAttack>().ammoCount)
            {
                case 3:
                    ammoCounter1.gameObject.SetActive(true);
                    ammoCounter2.gameObject.SetActive(true);
                    ammoCounter3.gameObject.SetActive(true);
                    break;
                case 2:
                    ammoCounter1.gameObject.SetActive(true);
                    ammoCounter2.gameObject.SetActive(true);
                    ammoCounter3.gameObject.SetActive(false);
                    break;
                case 1:
                    ammoCounter1.gameObject.SetActive(true);
                    ammoCounter2.gameObject.SetActive(false);
                    ammoCounter3.gameObject.SetActive(false);
                    break;
                case 0:
                    ammoCounter1.gameObject.SetActive(false);
                    ammoCounter2.gameObject.SetActive(false);
                    ammoCounter3.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

            barMask.transform.localPosition = new Vector2(Mathf.Lerp(barMaskEndingPosX, barMaskStartingPosX, _player.poweredUpTimer / _player.poweredUpTime), barMask.transform.localPosition.y);
        }
        else
        {
            ammoCounter1.gameObject.SetActive(false);
            ammoCounter2.gameObject.SetActive(false);
            ammoCounter3.gameObject.SetActive(false);

            timerBar.SetActive(false);
        }
    }
}
