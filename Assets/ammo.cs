using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OncollideAmmoBox : MonoBehaviour
{
    public float delayTime = 10.0f;
    public GameObject ammoTotal;
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            ammoTotal.GetComponent<TextMeshProUGUI>().text = (int.Parse(ammoTotal.GetComponent<TextMeshProUGUI>().text) + 30).ToString();
            
            Invoke("ReactivateObject", delayTime); 
        }
    }

    private void ReactivateObject()
    {
        gameObject.SetActive(true);
    }
}
