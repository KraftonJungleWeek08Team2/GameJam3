public interface IDamageable
{
    /// <summary>
    /// 데미지 받아서 체력 감소
    /// 피격 로직 실행
    /// </summary>
    /// <param name="amount"></param>
    void TakeDamage(int amount);
    
    /// <summary>
    /// 사망 확인
    /// 데미지 다 받은 후 처리하려고 분리
    /// </summary>
    void IsDie();
    
    /// <summary>
    /// 사망처리
    /// </summary>
    void Die();
}
