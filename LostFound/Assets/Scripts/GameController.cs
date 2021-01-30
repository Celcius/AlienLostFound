using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GrabbableItemVar chosenItem;

    [SerializeField]
    private GrabbableItemArrayVar itemPrefabs;

    [SerializeField]
    private BoxCollider2D[] spawnAreas;

    [SerializeField]
    private int maxSpawnPerArea = 3; 

    [SerializeField]
    private int itemsToGenerate = 9;

    [SerializeField]
    private IntVar successes;

    [SerializeField]
    private IntVar failures;

    [SerializeField]
    private GrabbableItemArrayVar instantiatedItems;

    [SerializeField]
    private DeliveryArea delivery;

     [SerializeField]
     private StringVar translatorString;

     [SerializeField]
     private FloatVar angerBar;

     [SerializeField]
     private Alien alien;

    private void Start() 
    {
        successes.Value = 0;
        failures.Value = 0;
        delivery.ClearArea();
        GrabbableItem[] items = CreateObjects(itemsToGenerate);
        instantiatedItems.Value = items;
        GenerateColors(items);
        NextAlien();
    }

    private string GeneratedString;

    private GrabbableItem[] CreateObjects(int amount)
    {
        if(amount == 0 || spawnAreas.Length == 0)
        {
            Debug.LogError("Trying to create empty");
            return new GrabbableItem[0];
        }
        amount = Mathf.Min(amount, spawnAreas.Length * maxSpawnPerArea);

        List<int> areaSlots = new List<int>();
        for(int i = 0; i < spawnAreas.Length; i++)
        {
            for(int j = 0; j < maxSpawnPerArea; j++)
            {
                areaSlots.Add(i);
            }
        }

        List<GrabbableItem> retItems = new List<GrabbableItem>(amount);

        for(int i = 0; i < amount; i++)
        {
            int chosenIndex = UnityEngine.Random.Range(0, areaSlots.Count);
            int areaIndex = areaSlots[chosenIndex];
            areaSlots.Remove(chosenIndex);

            retItems.Add(InstantiateGrabObject(spawnAreas[areaIndex].bounds));
        }

        return retItems.ToArray();
    }

    private GrabbableItem InstantiateGrabObject(Bounds spawnArea)
    {
        int chosenIndex = UnityEngine.Random.Range(0, itemPrefabs.Value.Length);
        GrabbableItem chosenObject = itemPrefabs.Value[chosenIndex];

        Vector2 position = (Vector3)GeometryUtils.RandomPointInBounds(spawnArea) + Vector3.up * chosenObject.transform.position.z;
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360.0f));
        return Instantiate<GrabbableItem>(chosenObject, position, rotation);
    }

    private void GenerateColors(GrabbableItem[] items)
    {
        RandomColorGeneration(items);
    }

    private void RandomColorGeneration(GrabbableItem[] items)
    {
        foreach(GrabbableItem item in items)
        {
            ObjectColor[] colors = new ObjectColor[item.SpriteCount];

            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = UnityEngineUtils.GetRandomEnumElement<ObjectColor>();
            }
            item.SetColors(colors);
        }
    }


    public void DeliverItem()
    {
        if(delivery.Contains(chosenItem.Value))
        {
            successes.Value++;
            Debug.Log("Success");
        }
        else
        {
            OnFailure();
        }

        foreach(GrabbableItem item in delivery.Items)
        {
            DestroyItem(item);
        }

        NextAlien();
    }

    private void OnFailure()
    {
        failures.Value++;
        Debug.Log("Failure");
    }
    
    public void QuestionButton()
    {
        angerBar.Value = Mathf.Clamp01(angerBar.Value + alien.AngerPerQuestion);
        if(!CheckAnger())
        {
            GenerateTranslatorString();
        }
        
    }

    private void DestroyItem(GrabbableItem item)
    {
        instantiatedItems.Remove(item);
        delivery.Remove(item);
        DestroyImmediate(item.gameObject);
    }
      
    public void RejectCustomer()
    {
        NextAlien();
    }

    private void ChooseNext()
    {
        angerBar.Value = 0;
        if(instantiatedItems.Value.Length == 0)
        {
            EndDay();
            return;
        }
        int chosenIndex = Random.Range(0, instantiatedItems.Value.Length);
        chosenItem.Value = instantiatedItems.Value[chosenIndex];
    }

    private void EndDay()
    {
        Debug.Log("Day Ended");
    }

    private void GenerateTranslatorString()
    {
        if(chosenItem.Value == null)
        {
            translatorString.Value = "";
        }

        ObjectShape[] shapes = chosenItem.Value.shapes;
        ObjectColor[] colors = chosenItem.Value.colors;

        string[] formats = {"I think it be {0} and {1}", 
                            "It's like totally {0} and like {1}",
                            "... {0} ... {1}...",
                            "Gimme {0} ... maybe {1}"};
        
        int shapeIndex = Random.Range(0, shapes.Length);
        int colorIndex = Random.Range(0, colors.Length);
        int formatIndex = Random.Range(0, formats.Length);
        
        translatorString.Value = string.Format(formats[formatIndex], shapes[shapeIndex], colors[colorIndex]);
    }

    private bool CheckAnger()
    {
        if(angerBar.Value >= 1.0f)
        {
            OnFailure();
            NextAlien();
            return true;
        }
        return false;
    }
    private void Update() 
    {
        angerBar.Value = Mathf.Clamp01(angerBar.Value + alien.AngerPerSecond * Time.deltaTime);
        CheckAnger();
    }

    private void NextAlien()
    {
        ChooseNext();
        angerBar.Value = 0;
        GenerateTranslatorString();
    }
}
