using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{
    [SerializeField] ClientGlobalInfo info;
    [SerializeField] Scrollbar sliderPlayerOne;
    [SerializeField] Scrollbar sliderPlayerTwo;
    [SerializeField] GameObject youOne;
    [SerializeField] GameObject youTwo;
    [SerializeField] GameObject oneWin;
    [SerializeField] GameObject oneLoose;
    [SerializeField] GameObject twoWin;
    [SerializeField] GameObject twoLoose;
    [SerializeField] AudioSource audioSourceBit;
    [SerializeField] AudioSource audioSourceWin;
    [SerializeField] AudioSource audioSourceLoose;
    [SerializeField] RawImage faceOneWin;
    [SerializeField] RawImage faceTwoWin;
    [SerializeField] RawImage faceOneLoose;
    [SerializeField] RawImage faceTwoLoose;
    [SerializeField] Transform mainParent;
    [SerializeField] Vector2 wait;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ScoreAnimation());
        if(info.playerId == 0)
        {
            youOne.SetActive(true);
            youTwo.SetActive(false);
        }
        else
        {
            youOne.SetActive(false);
            youTwo.SetActive(true);
        }
    }

    IEnumerator ScoreAnimation()
    {
        bool flipFlop = false;
        bool didWin = (info.playerOneScore > info.playerTwoScore && info.playerId == 0) || (info.playerOneScore < info.playerTwoScore && info.playerId == 1);
        while(info.playerOneScore + info.playerTwoScore > 0)
        {
            if (flipFlop)
            {
                if (info.playerOneScore > 0)
                {
                    DOTween.To(() => sliderPlayerOne.size, x => sliderPlayerOne.size = x, sliderPlayerOne.size + 1f / info.maxScore, 0.1f);
                    audioSourceBit.pitch += 0.03f;
                    audioSourceBit.Play();
                    info.playerOneScore--;
                    mainParent.DOShakeRotation(0.1f,10,3,20);
                    yield return new WaitForSeconds(Random.Range(wait.x, wait.y));
                }
            }
            else
            {
                if (info.playerTwoScore > 0)
                {
                    DOTween.To(() => sliderPlayerTwo.size, x => sliderPlayerTwo.size = x, sliderPlayerTwo.size + 1f / info.maxScore, 0.1f);
                    audioSourceBit.pitch += 0.03f;
                    audioSourceBit.Play();
                    info.playerTwoScore--;
                    mainParent.DOShakeRotation(0.1f, 10, 3, 20);
                    yield return new WaitForSeconds(Random.Range(wait.x, wait.y));
                }
            }
            flipFlop = !flipFlop;
        }

        if (info.playerId == 0)
        {
            info.skinOne = info.skin;
        }
        else
        {
            info.skinTwo = info.skin;
        }

        if (didWin)
        {
            audioSourceWin.Play();
            if(info.playerId == 0)
            {
                oneWin.SetActive(true);
                twoLoose.SetActive(true);
                faceOneWin.texture = info.skins[info.skinOne];
                faceTwoLoose.texture = info.skins[info.skinTwo];
            }
            else
            {
                oneLoose.SetActive(true);
                twoWin.SetActive(true);
                faceOneLoose.texture = info.skins[info.skinOne];
                faceTwoWin.texture = info.skins[info.skinTwo];
            }
        }
        else
        {
            audioSourceLoose.Play();
            if (info.playerId != 0)
            {
                oneWin.SetActive(true);
                twoLoose.SetActive(true);
                faceOneWin.texture = info.skins[info.skinOne];
                faceTwoLoose.texture = info.skins[info.skinTwo];
            }
            else
            {
                oneLoose.SetActive(true);
                twoWin.SetActive(true);
                faceOneLoose.texture = info.skins[info.skinOne];
                faceTwoWin.texture = info.skins[info.skinTwo];
            }
        }
    }
}
