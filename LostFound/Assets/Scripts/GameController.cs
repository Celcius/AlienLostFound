using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [SerializeField]
    private GrabbableItemVar chosenItem;

    [SerializeField]
    private GrabbableItemArrayVar itemPrefabs;

    [SerializeField]
    private BoxCollider2D[] spawnAreas;

    [SerializeField]
    private BoxCollider2D respawnArea;

    [SerializeField]
    private int maxSpawnPerArea = 3; 

    [SerializeField]
    private int itemsToGenerate = 9;

    [SerializeField]
    private IntVar successes;

    [SerializeField]
    private IntVar fakerDiscovered;

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
     private AlienScriptArrVar alienPrefabs;

     [SerializeField]
     private BoolVar IsGameEnabled;

    [SerializeField]
    private bool[] isFailureFaker;
    public bool[] IsFailureFaker => isFailureFaker;

    [SerializeField]
    private Transform alienParent;
    private Alien nextAlien;
    private Alien currentAlien;

    private bool hasStarted = false;

    private bool dayEnded = false;

    private float elapsedTime = 0;
    [SerializeField]
    private float timeToSpawn = 20.0f;
    [SerializeField]
    private float timeToSpawnDecPerc = 0.95f;
    [SerializeField]
    private float minTimetoSpawn = 12.5f;

    [SerializeField]
    private float fakerOdds = 0.2f;

    [SerializeField]
    private SoundSystem soundSystem;

    [SerializeField]
    private AudioClip electricity;

    [SerializeField]
    private Animator electricityAnim;

    [SerializeField]
    private GameObject shutterObject;

    private void Start() 
    {
        itemPrefabs.Reset();
        isFailureFaker = new bool[3]{false, false, false};
        IsGameEnabled.Value = false;
        nextAlien = GenerateAlien();
        successes.Value = 0;
        failures.Value = 0;
        fakerDiscovered.Value = 0;
        angerBar.Value = 0;
        delivery.ClearArea();
        GrabbableItem[] items = CreateObjects(itemsToGenerate, spawnAreas);
        instantiatedItems.Value = items;
        translatorString.Value = "";
        GenerateColors(items);
    }

    private string GeneratedString;

    private GrabbableItem[] CreateObjects(int amount, BoxCollider2D[] spawns)
    {
        if(amount == 0 || spawns.Length == 0)
        {
            Debug.LogError("Trying to create empty");
            return new GrabbableItem[0];
        }

        amount = Mathf.Min(amount, spawns.Length * maxSpawnPerArea);

        List<int> areaSlots = new List<int>();
        for(int i = 0; i < spawns.Length; i++)
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

            retItems.Add(InstantiateGrabObject(spawns[areaIndex].bounds));
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
        if(!delivery.ContainsAny())
        {
            OnFailure();
            return;
        }

         if(chosenItem.Value == null)
        {
            OnFailure(Alien.AlienAnimations.LeaveHappy, true);
        }
        else if(delivery.ContainsSimilar(chosenItem.Value))
        {
            AnimateAlienLeave(Alien.AlienAnimations.LeaveHappy);
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
    }

    private void OnFailure(Alien.AlienAnimations leaveAnim = Alien.AlienAnimations.LeaveAngry, bool isFake = false)
    {
        AnimateAlienLeave(leaveAnim);
        isFailureFaker[failures.Value] = isFake;
        failures.Value++;
        if(failures.Value == 3)
        {
            EndDay();
        }
        Debug.Log("Failure");
    }

    private void AnimateAlienLeave(Alien.AlienAnimations animType)
    {
        translatorString.Value = "";
        IsGameEnabled.Value = false;
        currentAlien.Animate(animType,
            () =>
            {
                NextAlien();
            });
    }
    
    public void QuestionButton()
    {

        angerBar.Value = Mathf.Clamp01(angerBar.Value + currentAlien.AngerPerQuestion);
        if(!CheckAnger())
        {
            translatorString.Value = "";
            currentAlien.Animate(Alien.AlienAnimations.Talk, 
                () => 
                {
                    GenerateTranslatorString();
                });
        }
        
    }

    private void DestroyItem(GrabbableItem item)
    {
        instantiatedItems.Remove(item);
        delivery.Remove(item);
        DestroyImmediate(item.gameObject);

        if(instantiatedItems.Count() <= 0)
        {
            EndDay(true);
        }
    }
      
    public void RejectCustomer()
    {
        soundSystem.PlaySound(electricity, "Zap", true);
        electricityAnim.SetTrigger("AnimateElectricity");
        if(chosenItem.Value == null)
        {
            AnimateAlienLeave(Alien.AlienAnimations.LeaveHurt);
            fakerDiscovered.Value++;
        }
        else
        {
            OnFailure(Alien.AlienAnimations.LeaveHurt);
        }
    }

    private void ChooseNextItem()
    {
        angerBar.Value = 0;
        if(instantiatedItems.Value.Length == 0)
        {
            EndDay();
            return;
        }

        float roll = Random.Range(0.0f,1.0f);
        if(roll <= fakerOdds)
        {
            Debug.Log("ALien is FAKER -> " + roll);
            chosenItem.Value = null;
        }
        else
        {
            int chosenIndex = Random.Range(0, instantiatedItems.Value.Length);
            chosenItem.Value = instantiatedItems.Value[chosenIndex];
        }
    }

    private void EndDay(bool shiftEnd = false)
    {
        if(dayEnded)
        {
            return;
        }

        dayEnded = true;
        IsGameEnabled.Value = false;

        shutterObject.SetActive(true);
        string[] firedStr = new string[] 
        {
            "Three Strikes!\nYou're Fired!",
            "Congrats!\nYou're Fired",
            "Get your stuff kid...\nYou're Fired!",
        };
        int firedIndex = Random.Range(0, firedStr.Length);

        translatorString.Value = shiftEnd? "Congrats, done the day!" : firedStr[firedIndex];

        StartCoroutine(WaitForTime());

        Debug.Log("Day Ended");
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(10.0f);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void GenerateTranslatorString()
    {
        string generatedString = "";

        if(chosenItem.Value == null)
        {
            generatedString = GenerateFakeString();
        }
        else
        {
            generatedString = CreateItemString(chosenItem.Value.shapes, 
                                               chosenItem.Value.colors);
        }
        translatorString.Value = generatedString;
    }

    private string CreateItemString(ObjectShape[] shapes, ObjectColor[] colors)
    {
        string format = currentAlien.GetTextFormat();
        List<int> indexes = new List<int>();
        for(int i = 0; i < shapes.Length; i++)
        {
            indexes.Add(i);
        }
        int shapeIndex = indexes[Random.Range(0, indexes.Count)];
        indexes.RemoveAt(shapeIndex);
        int shapeIndex2 = indexes[Random.Range(0, indexes.Count)];
        int colorIndex = Random.Range(0, colors.Length);

        string shapeStr = ObjectType.ShapeNameFromType(shapes[shapeIndex]);
        string shapeStr2 = ObjectType.ShapeNameFromType(shapes[shapeIndex2]);
        string colorStr = ObjectType.ColorNameFromType(colors[colorIndex]);

        shapeStr = currentAlien.JumbleText? 
                   UnityEngineUtils.CreateAnagram(shapeStr) :
                   shapeStr;

        shapeStr2 = currentAlien.JumbleText? 
            UnityEngineUtils.CreateAnagram(shapeStr2) :
            shapeStr2;

        colorStr = currentAlien.JumbleText? 
                   UnityEngineUtils.CreateAnagram(colorStr) :
                   colorStr;


        return string.Format(format, shapeStr, shapeStr2, colorStr);
    }


    private string GenerateFakeString()
    {
        HashSet<ObjectShape> allShapes = new HashSet<ObjectShape>();
        HashSet<ObjectColor> allColors = new HashSet<ObjectColor>();
        
        foreach(GrabbableItem item in instantiatedItems.Value)
        {
            foreach(ObjectShape shape in item.shapes)
            {
                allShapes.Add(shape);
            }

            foreach(ObjectColor color in item.colors)
            {
                allColors.Add(color);
            }    
        }

        List<ObjectShape> finalShapes = new List<ObjectShape>(allShapes);
        List<ObjectColor> finalColors = new List<ObjectColor>(allColors);
        
        return CreateItemString(finalShapes.ToArray(), finalColors.ToArray());

    }


    private bool CheckAnger()
    {
        if(!IsGameEnabled.Value)
        {
            return false;
        }
        
        if(angerBar.Value >= 1.0f)
        {
            IsGameEnabled.Value = false;
            OnFailure();

            return true;
        }
        return false;
    }
    private void Update() 
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > timeToSpawn)
        {
            SpawnSingleObject();
            elapsedTime = 0;
        }
        if(!hasStarted)
        {
            NextAlien();
            hasStarted = true;
        }
        
        if(IsGameEnabled.Value)
        {
            angerBar.Value = Mathf.Clamp01(angerBar.Value + currentAlien.AngerPerSecond * Time.deltaTime);
            CheckAnger();
        }
    }

    private void SpawnSingleObject()
    {
       CreateObjects(1, new BoxCollider2D[]{respawnArea});
       timeToSpawn = Mathf.Max(timeToSpawn*timeToSpawnDecPerc, minTimetoSpawn);
    }

    private void NextAlien()
    {
       if(dayEnded)
       {
           return;
       }

       currentAlien = nextAlien;
       nextAlien = GenerateAlien();

        currentAlien.Animate(Alien.AlienAnimations.MoveToFront,
            () =>
            {
                ChooseNextItem();
                angerBar.Value = 0;
                currentAlien.Animate(Alien.AlienAnimations.Talk, 
                () => 
                {
                    IsGameEnabled.Value = !dayEnded;
                    GenerateTranslatorString();
                });
            });
        
    }

    private Alien GenerateAlien()
    {
        IsGameEnabled.Value = false;
        int index = UnityEngine.Random.Range(0,alienPrefabs.Value.Length);
        Alien prefab = alienPrefabs.Value[index];
        return Instantiate<Alien>(prefab, prefab.transform.position, prefab.transform.rotation, alienParent);
    }
}
