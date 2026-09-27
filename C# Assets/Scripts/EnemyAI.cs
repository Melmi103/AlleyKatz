void MeleeState()
{
    if (player == null) return;

    switch (currentState)
    {
        case EnemyState.Idle:
            // optional: only detect when within detectionRange
            if (detectionRange <= 0f || Vector3.Distance(transform.position, player.transform.position) < detectionRange)
            {
                MeleeRandomDistance = Randomizer.CreateRandomizer().NextFloat(0.5f, 50f);

                // CORRECT: target is along the direction from enemy -> player
                Vector3 dirToPlayer = player.transform.position - transform.position;
                dirToPlayer.z = 0f;
                meleeTargetPosition = transform.position + dirToPlayer.normalized * MeleeRandomDistance;

                AimAtPlayer();

                if (lockedon)
                    SetState(EnemyState.Attack);
            }
            break;

        case EnemyState.Attack:
            // ensure a speed is set
            MeleeSpeed = Randomizer.CreateRandomizer().NextFloat(5f, 20f);

            Vector3 toTarget = meleeTargetPosition - transform.position;
            toTarget.z = 0f;
            float dist = toTarget.magnitude;

            // move toward target and stop when close enough
            if (dist > 0.1f)
            {
                transform.position += toTarget.normalized * MeleeSpeed * Time.deltaTime;
                Debug.DrawLine(transform.position, meleeTargetPosition, Color.red, 0.1f);
            }
            else
            {
                SetState(EnemyState.Idle);
            }

            StartCoroutine(ResetStateAfterSeconds(1f));
            lockedon = false;
            timer = 1f;
            break;
    }
}