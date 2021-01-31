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

    private IEnumerator decreaseAnger = null;

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
            areaSlots.RemoveAt(chosenIndex);

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
        RandomColorNoRepeatGeneration(items);
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

    private void RandomColorNoRepeatGeneration(GrabbableItem[] items)
    {
        HashSet<ObjectColor> currentColors = new HashSet<ObjectColor>();
        foreach(GrabbableItem item in items)
        {
            ObjectColor[] colors = new ObjectColor[item.SpriteCount];
            currentColors.Clear();

            for(int i = 0; i < colors.Length; i++)
            {
                ObjectColor color;

                do 
                {
                    color = UnityEngineUtils.GetRandomEnumElement<ObjectColor>();
                }
                while(currentColors.Contains(color));

                colors[i] = color;
                currentColors.Add(color);
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
            SetTranslatorString(currentAlien.GetHappyText());
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
        SetTranslatorString(currentAlien.GetFakerText());
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
        DecreaseAnger();
        SetTranslatorString("");
        switch(animType)
        {
            case Alien.AlienAnimations.LeaveAngry:
                SetTranslatorString(currentAlien.GetAngryText());
                break;

            case Alien.AlienAnimations.LeaveHappy:
                break;

            case Alien.AlienAnimations.LeaveHurt:
                SetTranslatorString(currentAlien.GetPainText());
                break;

            default:
                break;
        }
        IsGameEnabled.Value = false;
        currentAlien.Animate(animType,
            () =>
            {
                if(!dayEnded)
                {
                   
                }
                
                NextAlien();
            });
    }

    private void DecreaseAnger()
    {
        if(decreaseAnger != null)
        {
            StopCoroutine(decreaseAnger);
        }
        decreaseAnger = DecreaseAngerRoutine();
        StartCoroutine(decreaseAnger);
    }

    private IEnumerator DecreaseAngerRoutine()
    {
        const float timeToDecrease = 1.2f;
        float startAnger = angerBar.Value;
        float delta = startAnger/timeToDecrease;

        while(angerBar.Value > 0)
        {
            yield return new WaitForEndOfFrame();
            angerBar.Value -= delta * Time.deltaTime;
        }

        decreaseAnger = null;
        angerBar.Value = 0;
    }

    private void SetTranslatorString(string newStr)
    {
        translatorString.Value = newStr;       
    }
    
    public void QuestionButton()
    {

        angerBar.Value = Mathf.Clamp01(angerBar.Value + currentAlien.AngerPerQuestion);
        if(!CheckAnger())
        {
            SetTranslatorString("");
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
            SetTranslatorString(currentAlien.GetPainText());
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

        SetTranslatorString(shiftEnd? "Congrats, done the day!" : firedStr[firedIndex]);

        StartCoroutine(WaitForTime());

        Debug.Log("Day Ended");
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
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
        SetTranslatorString(generatedString);
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
        int colorIndex2 = colorIndex;
        while(colorIndex2 == colorIndex && colors.Length > 1)
        {
            colorIndex2 = Random.Range(0, colors.Length);
        } 

        string shapeStr = ObjectType.ShapeNameFromType(shapes[shapeIndex]);
        
        string colorStr = ObjectType.ColorNameFromType(colors[colorIndex]);

        string bonusStr ="";

        shapeStr = currentAlien.JumbleShape? 
                   UnityEngineUtils.CreateAnagram(shapeStr) :
                   shapeStr;

        colorStr = currentAlien.JumbleColor? 
                   UnityEngineUtils.CreateAnagram(colorStr) :
                   colorStr;
        
        if(Random.Range(0,10) <= 5.0f || colorIndex  == colorIndex2)
        {
            bonusStr = ObjectType.ShapeNameFromType(shapes[shapeIndex2]);

        }
        else 
        {
            bonusStr = ObjectType.ColorNameFromType(colors[colorIndex2]);         
        }

        return string.Format(format, shapeStr, bonusStr, colorStr);
    }


    private string GenerateFakeString()
    {
        HashSet<ObjectShape> allShapes = ObjectType.GetAllShapes();
        HashSet<ObjectColor> allColors = ObjectType.GetAllColors();

        HashSet<ObjectShape> currentShapes = new HashSet<ObjectShape>();
        HashSet<ObjectColor> currentColors = new HashSet<ObjectColor>();


        foreach(GrabbableItem item in instantiatedItems.Value)
        {
            foreach(ObjectShape shape in item.shapes)
            {
                currentShapes.Add(shape);
                allShapes.Remove(shape);
            }

            foreach(ObjectColor color in item.colors)
            {
                currentColors.Add(color);
                allColors.Remove(color);
            }    
        }

        List<ObjectShape> finalShapes = new List<ObjectShape>(allShapes.Count < 2? currentShapes : allShapes);
        List<ObjectColor> finalColors = new List<ObjectColor>(allColors.Count < 2? currentColors : allColors);
        
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
       GrabbableItem[] created = CreateObjects(1, new BoxCollider2D[]{respawnArea});
       timeToSpawn = Mathf.Max(timeToSpawn*timeToSpawnDecPerc, minTimetoSpawn);

       RandomColorNoRepeatGeneration(created);
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
                SetTranslatorString("");
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
