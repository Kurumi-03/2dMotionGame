using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用以控制各个实体的特殊显示效果
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    Entity entity;
    [Header("Injury")]
    public float fxDuring;//受伤特效持续时间
    [SerializeField] Material fxMaterial;//受击闪动效果
    Material originMaterial;//原始材质效果
    [Header("CounterAttack")]
    [SerializeField] float tipTime;//受击提示闪烁时间

    [Header("Magic Effect")]
    [SerializeField] Color[] fireColor;//火焰状态特效颜色组
    [SerializeField] Color[] iceColor;//冰冻状态特效颜色组
    [SerializeField] Color[] lightningColor;//雷电状态特效颜色组
    [SerializeField] float blindTime;//状态特效闪烁时间

    [Header("Attack Effect")]
    [SerializeField] GameObject critAcctackEffect;//暴击特效
    [SerializeField] GameObject normalAcctackEffect;//普通攻击特效
    [SerializeField] float effectOffsetX;//特效偏移量X
    [SerializeField] float effectOffsetY;//特效偏移量Y
    [SerializeField] float effectRotation;//特效旋转角度

    [Header("Dash Effect")]
    [SerializeField] GameObject dashEffect;//冲刺特效
    [SerializeField] float dashEffectDuring;//冲刺特效持续时间
    [SerializeField] float dashEffectCd;//冲刺特效显示间隔
    [SerializeField] float dashFadeTime;//冲刺特效淡出时间

    [Header("StateFX")]
    [SerializeField] ParticleSystem fireEffect;//火焰特效
    [SerializeField] ParticleSystem iceEffect;//冰冻特效
    [SerializeField] ParticleSystem lightningEffect;//雷电特效
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originMaterial = sr.material;

        InitStateFX();
    }



    //受伤显示效果
    public void DamageEffect()
    {
        StartCoroutine(FlashEffect());
    }

    public IEnumerator FlashEffect()
    {
        sr.material = fxMaterial;
        yield return new WaitForSeconds(fxDuring);
        sr.material = originMaterial;
    }

    //使物体透明
    public void SetTransparent(bool isClear)
    {
        if (isClear)
        {
            sr.color = Color.clear;
            //确保设置中的血条显示
            if (GameManager.Instance.isShowPlayerHealth)
            {
                PlayerManager.Instance.player.hpUI.SetActive(false);
            }
        }
        else
        {
            sr.color = Color.white;
            if (GameManager.Instance.isShowPlayerHealth)
            {
                PlayerManager.Instance.player.hpUI.SetActive(true);
            }
        }
    }

    void RedBlind()
    {
        if (sr.color == Color.white)
        {
            sr.color = Color.red;
        }
        else if (sr.color == Color.red)
        {
            sr.color = Color.white;
        }
    }

    public void CounterAcctackTip()
    {
        InvokeRepeating("RedBlind", 0, tipTime);//在0秒后每0.2秒执行一次
    }

    void FireChange()
    {
        if (sr.color == fireColor[0])
        {
            sr.color = fireColor[1];
        }
        else
        {
            sr.color = fireColor[0];
        }
    }

    void IceChange()
    {
        if (sr.color == iceColor[0])
        {
            sr.color = iceColor[1];
        }
        else
        {
            sr.color = iceColor[0];
        }
    }

    void LightningChange()
    {
        if (sr.color == lightningColor[0])
        {
            sr.color = lightningColor[1];
        }
        else
        {
            sr.color = lightningColor[0];
        }
    }

    public void FireBlind(float time)
    {
        FireEffect(time);
        InvokeRepeating("FireChange", 0, blindTime);
        Invoke("CancelBlind", time);
    }

    public void IceBlind(float time, float percent)
    {
        IceEffect(time);
        InvokeRepeating("IceChange", 0, blindTime);
        Invoke("CancelBlind", time);
        GetComponent<Entity>().SlowAction(time, percent);
    }

    public void lightningBlind(float time)
    {
        LightningEffect(time);
        InvokeRepeating("LightningChange", 0, blindTime);
        Invoke("CancelBlind", time);
    }

    public void CancelBlind()
    {
        CancelInvoke();
        // GetComponent<Entity>().CancelInvoke();
        // GetComponent<Entity>().RecoverAction();
        sr.color = Color.white;
    }

    //暴击特效
    public void CritAttackEffect(Transform target, int dir)
    {
        //制作随机的位置旋转效果
        Vector3 randomPos = new Vector3(Random.Range(-effectOffsetX, effectOffsetX), Random.Range(-effectOffsetY, effectOffsetY), 0);
        Vector3 randomRot = new Vector3(0, 0, Random.Range(-effectRotation, effectRotation));
        GameObject effect = Instantiate(critAcctackEffect, target.position + randomPos, Quaternion.identity);
        effect.transform.Rotate(randomRot);
        //设置特效方向为目标反向
        effect.transform.localScale = new Vector3(dir, 1, 1);
        Destroy(effect, 0.5f);
    }

    //普通攻击特效
    public void NormalAttackEffect(Transform target)
    {
        //制作随机的位置旋转效果
        Vector3 randomPos = new Vector3(Random.Range(-effectOffsetX, effectOffsetX), Random.Range(-effectOffsetY, effectOffsetY), 0);
        Vector3 randomRot = new Vector3(0, 0, Random.Range(-effectRotation, effectRotation));
        GameObject effect = Instantiate(normalAcctackEffect, target.position + randomPos, Quaternion.identity);
        effect.transform.Rotate(randomRot);
        Destroy(effect, 0.5f);
    }

    public void DashEffect()
    {
        GameObject effect = Instantiate(dashEffect, transform.position, Quaternion.identity);
        effect.GetComponent<AfterImageFade>().SetUpData(dashEffectCd, dashEffectDuring, dashFadeTime,
            sr.sprite, entity.faceDir, entity.transform);
    }

    void InitStateFX()
    {
        fireEffect.gameObject.SetActive(false);
        iceEffect.gameObject.SetActive(false);
        lightningEffect.gameObject.SetActive(false);
        fireEffect.Stop();
        iceEffect.Stop();
        lightningEffect.Stop();
    }

    public void FireEffect(float time)
    {
        fireEffect.gameObject.SetActive(true);
        StartCoroutine(Fire(time));
    }
    IEnumerator Fire(float time)
    {
        fireEffect.Play();
        yield return new WaitForSeconds(time);
        fireEffect.Stop();
        fireEffect.gameObject.SetActive(false);
    }

    public void IceEffect(float time)
    {
        iceEffect.gameObject.SetActive(true);
        StartCoroutine(Ice(time));
    }
    IEnumerator Ice(float time)
    {
        iceEffect.Play();
        yield return new WaitForSeconds(time);
        iceEffect.Stop();
        iceEffect.gameObject.SetActive(false);
    }
    public void LightningEffect(float time)
    {
        lightningEffect.gameObject.SetActive(true);
        StartCoroutine(Lightning(time));
    }
    IEnumerator Lightning(float time)
    {
        lightningEffect.Play();
        yield return new WaitForSeconds(time);
        lightningEffect.Stop();
        lightningEffect.gameObject.SetActive(false);
    }
}
