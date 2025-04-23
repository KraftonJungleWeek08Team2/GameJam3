using UnityEngine;

public class BeatBarDisappear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, MusicManager.Instance.noteInterval * 4);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
