﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadObject : BuildableObject
{
    public FixedJoint2D joint;
    public static HeadObject activeHead;

    private void OnEnable()
    {
        activeHead = this;
    }

    protected void Start()
    {
        if (GameManager.instance.playMode)
        {
            foreach (BuildableObject bo in GetAllConnectedObjects())
            {
                ActionObject ao = bo as ActionObject;
                if (ao) { ao.enableInput = true; }
            }
        }
    }

    public override void ConnectPivotToObject(BuildableObject _otherObject, PivotObject _pivot)
    {
        base.ConnectPivotToObject(_otherObject, _pivot);

        joint = _otherObject.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = rb;
        joint.anchor = _otherObject.transform.InverseTransformPoint(_pivot.transform.position);
    }

    public override void DisconnectPivot(PivotObject _pivot)
    {
        base.DisconnectPivot(_pivot);

        Destroy(joint);
    }

    public override void SetHighlight(Outlines? _outlineType)
    {
        base.SetHighlight(_outlineType);
        BuildableObject[] bOs = GetAllConnectedObjects();
        for (int i = 0; i < bOs.Length; i++)
        {
            if (_outlineType == Outlines.Highlighted) { bOs[i].SetHighlight(Outlines.Connected); }
            else { bOs[i].SetHighlight(null); }
        }
    }
}