using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfWarBuilders.Entities;

public abstract class GenericInteractable : MonoBehaviour
{
    HashSet<GenericInteractor> interactors = new HashSet<GenericInteractor>();

    public enum exclude { nothing , player }
    public exclude excludes; 

    public void OnTriggerEnter(Collider other)
    {
        var interactor = other.GetComponent<GenericInteractor>();

        if (interactor != null)
        {
            if (excludes == exclude.player)
            {
                var player = interactor.GetComponent<PlayerModel>();
                if (player != null) return;
            }

            OnBeginOverlapInteract();
            interactors.Add(interactor);
            interactor.AddInteractable(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var interactor = other.GetComponent<GenericInteractor>();

        if (interactor != null)
        {
            if (excludes == exclude.player)
            {
                var player = interactor.GetComponent<PlayerModel>();
                if (player != null) return;
            }

            OnEndOverlapInteract();
            interactors.Remove(interactor);
            interactor.RemoveInteractable(this);
        } 
    }

    private void OnDestroy()
    {
        foreach (var i in interactors)
        {
            i.RemoveInteractable(this);
        }
        interactors.Clear();
    }
    private void OnDisable()
    {
        foreach (var i in interactors)
        {
            i.RemoveInteractable(this);
        }
        interactors.Clear();
    }

    public void Execute()
    {
        OnExecute();
    }

    protected abstract void OnBeginOverlapInteract();
    protected abstract void OnEndOverlapInteract();
    protected abstract void OnExecute();
}
