using UnityEngine;

public class HasHealth : MonoBehaviour {
	int maxHealth = 100;
	public int currentHealth;
	public bool friendlyFire = false;
	public delegate void deathFunction();
	public deathFunction death;
	bool alive = true;
	bool sourceIsMe = false;
	BlastAgent agent;

	void Start () {
		currentHealth = maxHealth;
		agent = gameObject.GetComponent<BlastAgent>();
	}

	public void takeDamage (int amount, GameObject source) {
		// prevent taking damage and distributing rewards when dead
		if (alive) {
			// agent that damages us
			BlastAgent ba = source.GetComponent<BlastAgent>();

			// just in case of invalid damage source. this should never happen.
			if (ba == null) {
				return;
			}
			sourceIsMe = (source == gameObject);

			// Did I hit myself?
			if (sourceIsMe) {
				// Punish for self-damage
				if (!friendlyFire)
					return;
				ba.IFailedAPartialGoal();
			} else {
				// Reward for hitting an opponent
				ba.IScoredAPartialGoal();
			}
			// take damage
			currentHealth -= amount;

			// Am I dead?
			if (currentHealth <= 0) {
				// Did I shoot myself?
				if (!sourceIsMe) {
					// reward agent for scoring a kill
					ba.IScoredAGoal();
				}
				// punish for death, reset agent
				agent.IFailed();
				// legacy
				alive = false;
			}
		}
	}

	public void reset () {
		currentHealth = maxHealth;
		alive = true;
	}
}
