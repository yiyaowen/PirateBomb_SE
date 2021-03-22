using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Door : MonoBehaviour
{
    Animator animator;
    BoxCollider2D coll;

    public enum Action
    {
        motionless = 0,
        opening = 1,
        closing = 2
    }

    [Header("State Settings")]
    public bool isOpenOriginally;
    public Action initialAction;
    public float actionDelaySeconds;

    [Header("Trigger Events")]
    public List<string> methodNames = new List<string>();
    public List<MethodInfo> methods { get; set; } = new List<MethodInfo>();

    void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        coll.enabled = false;

        var availableMethods = typeof(GameManager).GetMethods();
        foreach (var method in availableMethods)
        {
            if (method.GetParameters().Length == 0 && methodNames.Contains(method.Name))
                methods.Add(method);
        }

        animator.SetBool("is_open_originally", isOpenOriginally);
        StartCoroutine("PostponeAction");
    }

    IEnumerator PostponeAction()
    {
        yield return new WaitForSeconds(actionDelaySeconds);
        if (initialAction == Action.opening)
            animator.SetTrigger("opening");
        else if (initialAction == Action.closing)
            animator.SetTrigger("closing");
    }

    public void Open()
    {
        animator.SetTrigger("opening");
        coll.enabled = true;
    }

    public void Close()
    {
        animator.SetTrigger("closing");
        coll.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (var method in methods)
            {
                method.Invoke(GameManager.instance, null);
            }
        }
    }
}
