﻿using System.Collections;
using UnityEngine;
using UnityStandardAssets._2D;

public class SceneMgt : MonoBehaviour
{
    [SerializeField] private Dialogue m_CurrentDialogue;
    private DialogueMgt m_DialogueManager;
    private AudioManager m_AudioManager;
    private CameraShake m_CameraShake;
    private Camera2DFollow m_CameraFollow;

    //specific members, to be removed in case of new import
    private PlayerMovement m_PlayerMov;
    private Player m_Player;
    [SerializeField] private GameObject m_BurnEffectPrefab;
    [SerializeField] private GameObject m_FirstDragon;
    [SerializeField] private GameObject m_SecondDragon;
    [SerializeField] private GameObject m_SausageObject;
    [SerializeField] private Collider2D m_SausageTrigger;
    [SerializeField] private SpriteRenderer m_SausageImage;
    [SerializeField] private GameObject m_CoinCounter;
    [SerializeField] private GameObject m_Barrier1;
    [SerializeField] private GameObject m_Barrier2;
    [SerializeField] private GameObject m_ArrowRed;


    // Start is called before the first frame update
    void Start()
    {
        m_DialogueManager = DialogueMgt.instance;
        m_AudioManager = AudioManager.instance;
        m_CameraShake = CameraShake.instance;
        m_CameraFollow = Camera.main.GetComponentInParent<Camera2DFollow>();
    }

    // Specific methods may have to be re written in cas of new import
    public void SpecificStartDialogue(Dialogue dialogue)
    {
        m_Barrier1.SetActive(true);
        m_CurrentDialogue = dialogue;
        m_AudioManager.CrossFade("Music", "MusicSorcerer", 3f, 3f, 0.5f);


        m_Player = FindObjectOfType<Player>();
        if (m_Player == null)
        {
            Debug.LogError(name + " no Player found");
        }
        else
        {
            m_PlayerMov = m_Player.GetComponent<PlayerMovement>();
            if (m_PlayerMov == null)
                Debug.LogError(name + " no PlayerMov found");
        }

        //specific operations
        m_PlayerMov.IsMovementAllowed = false;

    }

    // Specific methods may have to be re written in cas of new import
    public IEnumerator SpecificSceneAction(int sceneIndex)
    {

        switch (sceneIndex)
        {
            case 27:
                m_CameraFollow.target = transform.parent;
                break;
            case 26:
                m_DialogueManager.ShutOffContinueButton();
                yield return new WaitForSeconds(3f);
                m_AudioManager.PlaySound("Fire");
                m_CameraShake.Shake(0.02f, 1.25f);
                yield return new WaitForSeconds(1.17f);
                yield return new WaitForSeconds(0.2f);
                m_AudioManager.StopSound("Fire");
                yield return new WaitForSeconds(0.38f);
                m_AudioManager.PlaySound("DragonFire");
                yield return new WaitForSeconds(1.35f);
                m_AudioManager.StopSound("DragonFire");
                m_AudioManager.PlaySound("DragonDie");
                yield return new WaitForSeconds(0.07f);
                m_CameraShake.Shake(0.5f, 0.2f);
                m_AudioManager.PlaySound("WoodImpact");
                yield return new WaitForSeconds(m_CurrentDialogue.scenes[1].sceneClip.length - 6.17f);
                Destroy(m_FirstDragon);
                Destroy(m_SecondDragon);
                m_DialogueManager.DisplayNextSentence();
                m_DialogueManager.ShutOnContinueButton();
                break;

            case 11:
                if (m_CoinCounter != null)
                    m_CoinCounter.SetActive(true);
                m_AudioManager.PlaySound("Coin");
                break;

            case 4:
                m_DialogueManager.ShutOffContinueButton();
                yield return new WaitForSeconds(1f);

                GameObject burnprefab;

                m_PlayerMov.RollPlayer(true);

                burnprefab = Instantiate(m_BurnEffectPrefab, m_Player.transform);
                ParticleSystem.MainModule main = m_BurnEffectPrefab.GetComponent<ParticleSystem>().main;
                main.loop = true;
                main.duration = 3f;
                burnprefab.GetComponent<ParticleSystem>().Play();
                m_AudioManager.PlaySound("Fire");

                yield return new WaitForSeconds(3f);


                m_PlayerMov.RollPlayer(false);
                m_AudioManager.StopSound("Fire");
                Destroy(burnprefab);

                yield return new WaitForSeconds(1f);
                m_DialogueManager.ShutOnContinueButton();
                break;


            case 3:
                m_DialogueManager.ShutOffContinueButton();
                yield return new WaitForSeconds(0.52f);

                m_SausageTrigger.enabled = true;
                m_SausageImage.enabled = true;
                yield return new WaitForSeconds(0.5f);
                m_PlayerMov.IsMovementAllowed = true;
                PlayerSpit playerspit = FindObjectOfType<PlayerSpit>();
                if (playerspit != null)
                    playerspit.IsSpittingAllowed = true;
                m_ArrowRed.SetActive(true);


                break;

            case 2:

                m_DialogueManager.ShutOffContinueButton();
                m_ArrowRed.SetActive(false);
                m_PlayerMov.IsMovementAllowed = false;
                PlayerSpit playerspit2 = FindObjectOfType<PlayerSpit>();
                if (playerspit2 != null)
                    playerspit2.IsSpittingAllowed = false;
                yield return new WaitForSeconds(1.5f);
                m_SausageTrigger.enabled = false;
                m_SausageImage.enabled = false;
                m_DialogueManager.ShutOnContinueButton();
                yield return new WaitForSeconds(2f);
                Destroy(m_SausageObject);

                break;

            case 1:

                yield return new WaitForSeconds(0.25f);
                m_AudioManager.PlaySound("WoodImpact");
                yield return new WaitForSeconds(0.75f);
                m_AudioManager.PlaySound("WoodImpact");
                yield return new WaitForSeconds(0.5f);
                m_AudioManager.PlaySound("WoodImpact");
                break;

            default:
                break;
        }


        yield return null;
    }


    // Specific methods may have to be re written in cas of new import
    public void SpecificEndDialogue()
    {
        //specific operations
        m_PlayerMov.IsMovementAllowed = true;
        PlayerSpit playerspit3 = FindObjectOfType<PlayerSpit>();
        if (playerspit3 != null)
            playerspit3.IsSpittingAllowed = true;
        m_CameraFollow.target = m_Player.transform;
        m_AudioManager.CrossFade("MusicSorcerer", "Music", 3f, 3f, 0.1f);
        GameMaster.gm.m_IntroSceneEnded = true;
        Destroy(m_Barrier1);
        Destroy(m_Barrier2);
    }
}
