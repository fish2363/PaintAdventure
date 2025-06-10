using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureSOList", menuName = "SO/GestureSOList")]
public class GestureSOList : ScriptableObject
{
    public List<GestureSO> gestureSO = new();
}
