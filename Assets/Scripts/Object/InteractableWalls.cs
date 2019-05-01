using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWalls : MonoBehaviour, IActivable
{
    public bool isActive { get; set; }

    public bool isDown;

    [Tooltip("list of activated objects needed to open the door")]
    public List<GameObject> objectsConditions;

    private Animator anim;

	public SoundEmitter soundEmitter;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        if (isDown)
        {
            anim.SetBool("isDown", true);
            isActive = true;
            isDown = true;
        }
    }

    public void Activate()
    {
        if (CheckValidObjects())
        {
            if (isActive == false)
            {
				soundEmitter.PlaySound(0);
                anim.SetBool("isDown", true);
                isActive = true;
                isDown = true;
            }
            else if (isActive == true)
			{
				soundEmitter.PlaySound(0);
				anim.SetBool("isDown", false);
                isActive = false;
                isDown = false;
            }
        }
    }


    bool CheckValidObjects()
    {
        for (int i = 0; i < objectsConditions.Count; i++)
        {
            if (objectsConditions[i].GetComponent<IActivable>().isActive != true)
            {
                return false;
            }
        }
        return true;
    }

}
