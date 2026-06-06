using UnityEngine;
using UnityEngine.AI;

public class AnimationBlendController : MonoBehaviour
{

    [SerializeField] AnimationClip _defaultAnim;
    [SerializeField] AnimationClip _blendAnim;
    [SerializeField] AnimationClip _walkAnim;


    Animator _animator;
    AnimationSystem _animSystem;
    NavMeshAgent _agent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_animator = GetComponent<Animator>();
        //_animSystem = new AnimationSystem(_animator, _blendAnim, _walkAnim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
