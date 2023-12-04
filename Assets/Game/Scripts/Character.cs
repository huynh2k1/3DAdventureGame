using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class Character : MonoBehaviour
{

    #region Variables
    //Player
    public Animator animator;
    public CharacterController _cc;
    public CharacterState CurrentState;
    public PlayerVFXManager playerVFXManager;
    //PlayerInput playerInput;

    public float moveSpeed = 5f;
    public float gravity = -9.8f;
    Vector3 _movementVelocity; // Vận tốc 
    float _verticalVelocity;


    //Enemy
    public bool isPlayer = true;
    UnityEngine.AI.NavMeshAgent _navMeshAgent; //Component tự động phát hiện mục tiêu 
    Transform _targetPlayer;


    //Player sliders
    private float _attackStartTime;
    public float AttackSlideDuration = 0.4f;//Thời lượng slide
    public float AttackSlideSpeed = 0.06f; //Tốc độ slide

    //Health
    public Health health;

    //DamageCaster
    public DamageCaster damageCaster;

    //Material animation
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    //Item health
    public GameObject healthOrbObj;

    //Direction impact
    public Vector3 impactOnCharacter; //hướng lực đẩy lên character khi bị tấn công

    //Player Invicible
    public bool isInvincible;
    public float invincibleDuration = 2f;

    //Time anim attack
    private float attackAnimationDuration;

    //Dash
    public float slideSpeed = 5f;

    //Spawn
    public float spawnDuration = 2f;
    private float _currentSpawnTime;
    #endregion

    #region Functions
    private void Awake()
    {
        health = GetComponent<Health>();
        damageCaster = GetComponentInChildren<DamageCaster>();

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

        if (!isPlayer) // Neu la enemy
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _targetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = moveSpeed;
            SwitchStateTo(CharacterState.Spawn);
        }
        //else //Neu la player
        //{
        //    playerInput = GetComponent<PlayerInput>();
        //}
    }


    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case CharacterState.Normal:
                if (isPlayer)
                {
                    CalculatePlayerMovement();
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;
            case CharacterState.Attacking:

                if (isPlayer)
                {

                    if (Time.time < _attackStartTime + AttackSlideDuration)
                    {
                        float timePassed = Time.time - _attackStartTime;
                        float lerpTime = timePassed / AttackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, lerpTime);
                    }

                    if(PlayerInput.instance.mouseButtonDown && _cc.isGrounded)
                    {
                        string currentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        if(currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                        {
                            PlayerInput.instance.mouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);
                            CalculatePlayerMovement();
                        }
                    }
                }

                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:

                break;
            case CharacterState.Slide:
                _movementVelocity = transform.forward * slideSpeed * Time.deltaTime;
                break;
            case CharacterState.Spawn:
                _currentSpawnTime -= Time.deltaTime;
                if(_currentSpawnTime <= 0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
        }

        if (impactOnCharacter.magnitude > 0.2f) // nếu độ dài của vector > 0.2f
        {
            _movementVelocity = impactOnCharacter * Time.deltaTime;
        }
        impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);
        if (isPlayer)
        {
            if (_cc.isGrounded == false)
            {
                _verticalVelocity = gravity;
            }
            else
            {
                _verticalVelocity = gravity * 0.3f;
            }
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _cc.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
        }
        else
        {
            if(CurrentState != CharacterState.Normal)
            {
                _cc.Move(_movementVelocity);
                _movementVelocity = Vector3.zero;
            }
        }
    }


    //Tính toán di chuyển của player
    public void CalculatePlayerMovement()
    {
        if(PlayerInput.instance.mouseButtonDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        if(PlayerInput.instance.spaceKeyDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }


        _movementVelocity.Set(PlayerInput.instance.horizontalInput, 0f, PlayerInput.instance.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0f, -45f, 0f) * _movementVelocity;

        animator.SetFloat("Speed", _movementVelocity.magnitude);

        _movementVelocity *= moveSpeed * Time.deltaTime;

        if(_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        animator.SetBool("AirBorne", !_cc.isGrounded);


    }

    //Tính toán mục tiêu của enemy 
    public void CalculateEnemyMovement()
    {
        if(Vector3.Distance(_targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(_targetPlayer.position);
            animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            animator.SetFloat("Speed", 0f);

            SwitchStateTo(CharacterState.Attacking);
        }
    }

    //Chuyển đổi trạng thái
    public void SwitchStateTo(CharacterState newState)
    {
        if (isPlayer)
        {
            PlayerInput.instance.ClearCache();
        }
        switch(CurrentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if(damageCaster != null)
                {
                    DisableDamageCaster();
                }
                if (isPlayer)
                {
                    playerVFXManager.StopBlade();
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                isInvincible = false;
                break;
        }
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:

                if (!isPlayer)
                {
                    Quaternion newRotation = Quaternion.LookRotation(_targetPlayer.position - transform.position);
                    transform.rotation = newRotation;
                }

                animator.SetTrigger("Attack");

                if(isPlayer)
                {
                    _attackStartTime = Time.time;//lưu thời gian bắt đầu anim
                }

                break;
            case CharacterState.Dead:
                _cc.enabled = false;
                animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                animator.SetTrigger("BeingHit");
                if (isPlayer)
                {
                    isInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;
            case CharacterState.Slide:
                animator.SetTrigger("Slide");
                break;
            case CharacterState.Spawn:
                isInvincible = true;
                _currentSpawnTime = spawnDuration; //2s
                StartCoroutine(MasterialAppear());
                break;

        }
        CurrentState = newState;
    }

    //Chuyển đổi trạng thái từ attack -> normal khi kết thúc anim attack
    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    //Chuyển đổi trạng thái từ beinghit -> normal khi kết thúc anim beinghit
    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    //Chuyển đổi trạng thái từ slide -> normal khi kết thức anim slide
    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    //Gây dmg
    public void ApplyDamage(int damage, Vector3 attackPos = new Vector3())
    {
        if (isInvincible && isPlayer)
        {
            return;
        }

        if(health != null)
        {
            health.TakeDamage(damage);
        }

        if (!isPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackPos);
            AddImpact(attackPos, 2.5f);

        }
        else
        {
            SwitchStateTo(CharacterState.BeingHit);
            AddImpact(attackPos, 10f);
            CanvasController.instance.canvasGame.UpdateHealthBar(health.CurrentHealthPercentage());
        }

        StartCoroutine(MaterialBlink());

       
    }

    //AddForce impact player after beinghit
    private void AddImpact(Vector3 attackPos, float force)
    {
        Vector3 impartDir = transform.position - attackPos;
        impartDir.Normalize();
        impartDir.y = 0;
        impactOnCharacter = impartDir * force;
    }

    //Enable Collider
    public void EnableDamageCaster()
    {
        damageCaster.EnableDamageCaster();
    }

    //Disable Collider
    public void DisableDamageCaster()
    {
        damageCaster.DisableDamageCaster();
    }

    //Instantiate item health
    private void DropItemHp()
    {
        if(healthOrbObj != null)
        {
            Instantiate(healthOrbObj, transform.position, Quaternion.identity);
        }
    }

    //Pickup item
    public void PickUpItem(PickUp item)
    {
        switch (item.typeItem)
        {
            case TypeItem.Heal:
                AddHealth(item.Value);
                break;
            case TypeItem.Coin:
                AddCoin(item.Value);
                break;
        }
    }

    private void AddHealth(int value)
    {
        health.AddHealth(value);
        playerVFXManager.PlayHeal();
        CanvasController.instance.canvasGame.UpdateHealthBar(health.CurrentHealthPercentage());
    }

    private void AddCoin(int value)
    {
        
    }

    //Effect blink
    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink", 0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    //Effect dissolve
    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float dissolveTimeDuration = 2f; // thời gian chạy hiệu ứng từ bắt đầu đến khi kết thúc hiệu ứng
        float currentDissolveTime = 0f; // thời gian hiện tại 
        float dissolveHight_start = 20f; // 
        float dissolveHight_target = -10f;
        float dissolveHight;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while(currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime; //tăng time 
            dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);

            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

            yield return null;
        }

        DropItemHp();

    }

    //Effect Instantiate
    IEnumerator MasterialAppear()
    {
        float dissolveTimeDuration = spawnDuration;
        float currentDissolveTime = 0;
        float dissolveHight_start = -10f;
        float dissolveHight_target = 20f;
        float dissolveHight;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime; //tăng time 
            dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);

            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

            yield return null;
        }

        _materialPropertyBlock.SetFloat("_enableDissolve", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    //Countdown invincible
    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    public void RotateToTarget()
    {
        if (CurrentState != CharacterState.Dead)
        {
            transform.LookAt(_targetPlayer, Vector3.up);
        }
    }

    #endregion
}


public enum CharacterState
{
    Normal, 
    Attacking,
    Dead,
    BeingHit,
    Slide,
    Spawn
}
