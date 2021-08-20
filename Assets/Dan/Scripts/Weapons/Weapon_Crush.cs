using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Crush : MonoBehaviour {

    bool hasTouchedBottom = false;
    bool hasCycled = false;

    public bool move = false;

    public float spawnTime = 60f;


    public AudioClip[] thanosClips;
    private AudioSource aSource;


    private int currentClipId =0;

	void Start () {
        transform.position = new Vector3(Random.Range(5, 75), 0, 65);
        aSource = GetComponent<AudioSource>();
        StartCoroutine(Timer());
    }
	
	void Update () {
        if (!move)
            return;
        if (!hasTouchedBottom)
        {
            if (transform.position.z > 0)
            {
                transform.Translate(-Vector3.forward * 0.1f);                
            }
            else
            {
                aSource.clip = thanosClips[PickQuote()];
                aSource.Play();
                hasTouchedBottom = true;
            }
        }           
        else
        {
            if (transform.position.z < 45)
            {
                transform.Translate(Vector3.forward * 0.1f);                
            }
            else
            {    
                hasCycled = true;
            }
        }

	}

    int PickQuote()
    {
        int temp;
        do
        {
            temp = Random.Range(0, thanosClips.Length);
        } while (temp == currentClipId);
        currentClipId = temp;
        return currentClipId;
    }

    IEnumerator Timer()
    {
        hasTouchedBottom = false;        
        yield return new WaitForSeconds(spawnTime);
        aSource.clip = thanosClips[PickQuote()];
        aSource.Play();
        move = true;
        while (!hasCycled)
        {
            yield return new WaitForEndOfFrame();
        }
        move = false;
        transform.position = new Vector3(Random.Range(5, 75), 0, 45);
        hasCycled = false;
        StartCoroutine(Timer());
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>(),true);
        }
        TerrainChunk chunk = collision.gameObject.GetComponent<TerrainChunk>();
        if(chunk != null)
        {           
            chunk.ForceDestroy(collision, 5);
        }
        GameObject[] snails = GameObject.FindGameObjectsWithTag("Snail");
        foreach (GameObject snail in snails)
        {
            float distance = Vector3.Distance(transform.position, snail.transform.position);
            if (distance <= 5)
            {
                Snail snailProperties = snail.GetComponent<Snail>();
                snailProperties.health.DecreaseHealth(1000);
            }
        }
    }
}
