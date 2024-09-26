using Game;
using Game.Actions;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Replace the FirstName and LastName with your name and last name :)
/// </summary>
namespace Jo_Colomban
{
    public class Jo_Colomban_Controller : HeroController
    {
        public override ControllerAction Think()
        {
            return new Action_Wait(this, 1.0f);
        }
    }
}
