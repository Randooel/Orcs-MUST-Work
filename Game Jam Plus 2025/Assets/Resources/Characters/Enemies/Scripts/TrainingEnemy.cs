using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TrainingEnemy : EnemyBehavior
{
    #region Chase and Death overrides
    // Switch state to Idle once called
    private void Idle()
    {
        SwitchState(State.Idle);
        _animator.Play("Idle");
    }

    protected override void HandleChase()
    {
        Idle();
    }

    protected override void HandleDeath()
    {
        Idle();
    }
    #endregion
}
