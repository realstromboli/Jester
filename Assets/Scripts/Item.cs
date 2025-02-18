using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    public bool isCollected;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        data.collectedItems.TryGetValue(id, out isCollected);
        if (isCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.collectedItems.ContainsKey(id))
        {
            data.collectedItems.Remove(id);
        }
        data.collectedItems.Add(id, isCollected);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollected = true;
            gameObject.SetActive(false);
        }
    }
}
