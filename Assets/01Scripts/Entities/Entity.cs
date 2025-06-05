using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : ExtendedMono
{
    public bool IsDead { get; set; }
    protected Dictionary<Type, IEntityComponent> _components = new Dictionary<Type, IEntityComponent>();

    protected virtual void Awake()
    {
        AddComponents();
        InitializeComponents();
    }

    protected virtual void InitializeComponents()
    {
        _components.Values.ToList().ForEach(compo => compo.Initialize(this));
    }

    public virtual void AddComponents()
    {
        GetComponentsInChildren<IEntityComponent>().ToList()
            .ForEach(compo => _components.Add(compo.GetType(), compo));
    }


    public T GetCompo<T>() where T : IEntityComponent => (T)_components.GetValueOrDefault(typeof(T));
}
