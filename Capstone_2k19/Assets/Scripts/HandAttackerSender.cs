using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttackerSender : MonoBehaviour
{
    [SerializeField] private PlayerCombatController playerCombat = null;

    public void SignalFireBall() { playerCombat.ShootFireBall(); }
    public void SignalMeleeAttack() { playerCombat.MeleeAttack(); }
}
