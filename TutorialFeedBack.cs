using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFeedBack : MonoBehaviour
{
   [SerializeField] QuestManager qm;
   [SerializeField] move Player;
   [SerializeField] Rigidbody rb;
   [SerializeField] MyButton SprintButton;
   [SerializeField] Button JumpButton;
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject UIThing;
    public void NextQuest()
    {
        qm.CurrentQuestCompleted();
    }

    public void SetActiveThing()
    {
        if(!qm.ActiveQuest.isComplete)
        {
            UIThing.SetActive(false);
        }
        else
        {
            NextQuest();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TutorialComplete"))
        {
            
            qm.ChangeText("Good Job!\n\nTap The Screen To Continue");
            UIThing.SetActive(true);
            qm.ActiveQuest.isComplete = true;
        }
    }
    public void TutorialNumber2Check()
    {
        if (qm.currentQuestIndex == 1&&SceneManager.GetActiveScene().name.Equals("1_tutorial"))
        {
            if (Player.OnGround)
            {
                qm.ChangeText("Good Job!");
                UIThing.SetActive(true);
                qm.ActiveQuest.isComplete = true;
            }
        }
    }
    public void GoBackToMainGame(string s)
    {
        if(s.Equals("Jetpack"))
        {
            GameManager.LoadScene("Level_1");
            PlayerPrefs.SetInt("LevelToBeExecuted", 9);
            PlayerPrefs.SetInt("IsUnlocked9", 1);
        }
        else if(s.Equals("GrappleGun"))
        {
            GameManager.LoadScene("Level_1");
            PlayerPrefs.SetInt("LevelToBeExecuted", 5);
            PlayerPrefs.SetInt("IsUnlocked5", 1);
        }
    }
}
