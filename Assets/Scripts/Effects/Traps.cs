using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public GameObject trapOn;

    // Start is called before the first frame update
    void Start()
    {
        trapOn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        StartCoroutine("Delay");

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine("Delay");
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.5f);
        trapOn.SetActive(true);

        yield return new WaitForSeconds(1f);
        trapOn.SetActive(false);
    }
}
