using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmoaebaUtils;

[RequireComponent (typeof(Collider2D))]
[RequireComponent (typeof(OutlinedObject))]
[RequireComponent (typeof(Rigidbody2D))]
public class GrabbableItem : MonoBehaviour
{
    [SerializeField]
    private bool canGrab = true;
    public bool CanGrab => canGrab;

    [SerializeField]
    private GrabbableArea validArea;

    private Plane dragPlane;

    private Vector3 offset;

    private Camera myMainCamera; 

    private OutlinedObject outline;

    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Color grabColor;

    private bool isGrabbing = false;

    [SerializeField]
    private SpriteRenderer[] sprites;
    public int SpriteCount => sprites.Length;

    public ObjectColor[] colors;
    
    [SerializeField]
    public ObjectShape[] shapes;

    Rigidbody2D body;
    Collider2D col;

    void Start()
    {
        myMainCamera = Camera.main;
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        outline = GetComponent<OutlinedObject>();
        for(int i = 0; i < colors.Length; i++)
        {
            UpdateColor(colors[i], i);
        }
    }

    void OnMouseOver()
    {
        outline.UserCall(true);
    }

    void OnMouseExit()
    {
        if(!isGrabbing)
        {
            outline.UserCall(false);
        }
    }

    private void OnMouseDown() 
    {
        if(!CanGrab)
        {
            LetGo();
            return;
        }

        Grab();
        dragPlane = new Plane(myMainCamera.transform.forward, transform.position); 
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition); 

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist); 
        offset = transform.position - camRay.GetPoint(planeDist);
    }

    private void OnMouseUp() 
    {
        LetGo();
    }

    private void OnMouseDrag() 
    {
        if(!CanGrab)
        {
            LetGo();
            return;
        }

        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition); 

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        transform.position = ValidClampedPos(camRay.GetPoint(planeDist) + offset);
    }

    private Vector3 ValidClampedPos(Vector3 pos)
    {
        if(validArea == null)
        {
            pos.z = transform.position.z;
            return pos;
        }

        pos = Vector3.Max(validArea.bounds.min, pos);
        pos = Vector3.Min(validArea.bounds.max, pos);
        pos.z = transform.position.z;

        return pos;
    }

    private void LetGo()
    {
        isGrabbing = false;
        outline.outlineColor = hoverColor;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.isKinematic = false;
        col.enabled = true;
        
        outline.UserCall(false);
    }

    private void Grab()
    {
        isGrabbing = true;
        outline.outlineColor = grabColor;
        outline.UserCall(true);
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.isKinematic = true;
        col.enabled = false;
    }

    public bool ContainsColor(ObjectColor searchColor)
    {
        foreach(ObjectColor color in colors)
        {
            if(color == searchColor)
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainsShape(ObjectShape searchShape)
    {
        foreach(ObjectShape shape in shapes)
        {
            if(shape == searchShape)
            {
                return true;
            }
        }
        return false;
    }

    public void SetColors(ObjectColor[] colors)
    {
        this.colors = colors;
    }

    private void UpdateColor(ObjectColor colorType, int i)
    {
        if(i < 0 || i >= Mathf.Min(colors.Length, sprites.Length))
        {
            return;
        }

        this.colors[i] = colorType;

        SpriteRenderer sprite = sprites[i];
        Color color = ObjectType.ColorFromType(colorType);
        Color spriteColor = new Color(color.r, color.g, color.b, sprite.color.a);
        sprite.color = spriteColor;
    }

    public bool Similar(GrabbableItem item)
    {
        HashSet<ObjectColor> theirColors = new HashSet<ObjectColor>(item.colors);
        HashSet<ObjectShape> theirShapes = new HashSet<ObjectShape>(item.shapes);

        HashSet<ObjectColor> myColors = new HashSet<ObjectColor>(colors);
        HashSet<ObjectShape> myShapes = new HashSet<ObjectShape>(shapes);

        if(theirColors.Count != myColors.Count && theirShapes.Count != myShapes.Count)
        {
            return false;
        }

        foreach(ObjectColor color in theirColors)
        {
            if(!myColors.Contains(color))
            {
                return false;
            }
        }

        foreach(ObjectShape shape in theirShapes)
        {
            if(!myShapes.Contains(shape))
            {
                return false;
            }
        }

        return true;        
    }
}
