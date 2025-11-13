using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandler(short type);

public delegate void EventHandler<in T>(short type, T data);

public class EventDispatcher
{
    public static readonly EventDispatcher instance = new EventDispatcher();

    private EventDispatcher()
    {
    }

    private readonly Dictionary<short, List<Delegate>> _eventHandlerDic = new Dictionary<short, List<Delegate>>();

    public void AddEventHandler(short type, EventHandler handler)
    {
        AddEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler(short type, EventHandler handler)
    {
        RemoveEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler(short type)
    {
        if (_eventHandlerDic.ContainsKey(type))
        {
            _eventHandlerDic.Remove(type);
        }
    }


    public void RemoveAllEventHandlers()
    {
        _eventHandlerDic.Clear();
    }

    public void AddEventHandler<T>(short type, EventHandler<T> handler)
    {
        AddEventHandlerInternal(type, handler);
    }

    public void RemoveEventHandler<T>(short type, EventHandler<T> handler)
    {
        RemoveEventHandlerInternal(type, handler);
    }

    public void SendEvent(short type)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            foreach (var handler in eventHandlers)
            {
                if (handler is EventHandler typedHandler)
                {
                    typedHandler(type);
                }
            }
        }
    }

    public void SendEvent<T>(short type, T msg)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            foreach (var handler in eventHandlers)
            {
                if (handler is EventHandler<T> typedHandler)
                {
                    typedHandler(type, msg);
                }
            }
        }
    }

    public int GetEventHandlerCount(short type)
    {
        var eventHandlers = GetEventHandlers(type);
        if (eventHandlers != null)
        {
            return eventHandlers.Count;
        }

        return 0;
    }


    public void Clear()
    {
        _eventHandlerDic.Clear();
    }

    private void AddEventHandlerInternal(short type, Delegate handler)
    {
        if (_eventHandlerDic.TryGetValue(type, out var eventHandlers))
        {
            if (!eventHandlers.Contains(handler))
            {
                eventHandlers.Add(handler);
            }
        }
        else
        {
            eventHandlers = new List<Delegate> { handler };
            _eventHandlerDic.Add(type, eventHandlers);
        }
    }

    private void RemoveEventHandlerInternal(short type, Delegate handler)
    {
        if (_eventHandlerDic.TryGetValue(type, out var eventHandlers))
        {
            eventHandlers.Remove(handler);
            if (eventHandlers.Count == 0)
            {
                _eventHandlerDic.Remove(type);
            }
        }
    }

    private List<Delegate> GetEventHandlers(short type)
    {
        if (_eventHandlerDic.TryGetValue(type, out var eventHandlers))
        {
            return eventHandlers;
        }

        return null;
    }
}