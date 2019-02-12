using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivable
{
    bool isActive { get; set; }
    void Activate();
}
