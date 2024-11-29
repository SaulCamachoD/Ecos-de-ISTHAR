using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveBoss : MonoBehaviour
{
   [SerializeField] private Transform player;
   [SerializeField] private float speed;
   private NavMeshAgent _navMeshAgent;


   private void Awake()
   {
      _navMeshAgent = GetComponent<NavMeshAgent>();
      _navMeshAgent.speed = speed;
   }

   private void Update()
   {
      MoveTowardPLayer();
   }

   private void MoveTowardPLayer()
   {
         _navMeshAgent.SetDestination(player.position);   
      
   }
   
   
}