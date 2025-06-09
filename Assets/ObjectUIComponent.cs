using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectUIComponent : MonoBehaviour,IEntityComponent
{
    private Dictionary<string, ObjUI> objUIs = new();

    public ObjUI GetObjUI(string name)
    {
        return objUIs[name];
    }

    public void Initialize(Entity entity)
    {
        GetComponentsInChildren<ObjUI>().ToList()
            .ForEach(value => objUIs.Add(value.name, value));
    }
}
