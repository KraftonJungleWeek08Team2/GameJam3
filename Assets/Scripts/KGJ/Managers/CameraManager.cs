using Unity.Cinemachine;
using UnityEngine;

public class CameraManager
{
    Player _player;
    CinemachineTargetGroup _targetGroup;
    CinemachineCamera _moveCamera;
    CinemachineCamera _battleCamera;

    /// <summary>
    /// 타겟 그룹에 플레이어를 추가합니다.
    /// </summary>
    public void Init()
    {
        _player = GameObject.FindAnyObjectByType<Player>();
        _targetGroup = GameObject.FindAnyObjectByType<CinemachineTargetGroup>();
        _moveCamera = GameObject.FindAnyObjectByType<MoveCamera>().GetComponent<CinemachineCamera>();
        _battleCamera = GameObject.FindAnyObjectByType<BattleCamera>().GetComponent<CinemachineCamera>();
        _shakeImpulse = GameObject.FindAnyObjectByType<CameraShakeEffect>().GetComponent<CinemachineImpulseSource>();

        ChangeBattleCamera(false);
        AddMember(_player.transform, 0.5f, 1f);
    }

    public void ChangeBattleCamera(bool isBattle)
    {
        if (isBattle)
        {
            _battleCamera.gameObject.SetActive(true);
            _moveCamera.gameObject.SetActive(false);
        }
        else
        {
            _moveCamera.gameObject.SetActive(true);
            _battleCamera.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 중복 체크 후 타겟 그룹에 멤버를 추가합니다.
    /// </summary>
    public void AddMember(Transform target, float radius, float weight)
    {
        if (!ContainsTarget(target))
        {
            _targetGroup.AddMember(target, weight, radius);
        }
    }

    /// <summary>
    /// 존재 체크 후 타겟 그룹에 멤버를 제거합니다.
    /// </summary>
    public void RemoveMember(Transform target)
    {
        if (ContainsTarget(target))
        {
            _targetGroup.RemoveMember(target);
        }
    }

    bool ContainsTarget(Transform newTarget)
    {
        foreach (var target in _targetGroup.Targets)
        {
            if (target.Object.transform == newTarget.gameObject.transform)
            {
                return true;
            }
        }
        return false;
    }
}
