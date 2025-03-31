using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//枚举类型方便查找状态
public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death
}

//建立一个储存敌人信息的类便于管理
[Serializable]
public class Parameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public Animator animator;
    public bool getHit;
}
public class FSM : MonoBehaviour
{

    private IState currentState;
    //实例化一个字典
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    //使得这个类可以在监视面板编辑参数
    public Parameter parameter;
    void Start()
    {
        //字典就是键值对集合，通过key找到value
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        //厨师状态为Idle
        TransitionState(StateType.Idle);

        parameter.animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        currentState.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            parameter.getHit = true;
        }
    }
    
    //切换状态函数
    public void TransitionState(StateType type)
    {
        //转移状态前需要执行一次前一个状态的OnExit函数
        if (currentState != null)
            currentState.OnExit();
        //将当前状态切换为指定状态
        currentState = states[type];
        currentState.OnEnter();
    }

    //根据目标位置反转
    public void FlipTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }
}