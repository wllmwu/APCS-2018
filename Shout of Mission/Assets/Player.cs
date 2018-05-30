using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : Entity {

  public Slider healthBar;
  float previousHealth;
  public Text scoreText;
  public Text finalScoreText;

  int score;

  public override void TakeDamage(float amount) {
    base.TakeDamage(amount);
    UpdateHealthBar();
    StartCoroutine(TryRegeneration(health));
  }

  IEnumerator TryRegeneration(float prevHealth) {
    yield return new WaitForSeconds(3f);
    if (health == prevHealth) {
      previousHealth = health;
      StartCoroutine("Regenerate");
    }
    yield return null;
  }

  IEnumerator Regenerate() {
    while (health < 100f && health == previousHealth) {
      health++;
      previousHealth = health;
      UpdateHealthBar();
      yield return new WaitForSeconds(0.1f);
    }
    yield return null;
  }

  public override void Die() {
    Debug.Log("die");
    GetComponent<CharacterController>().enabled = false;
    GetComponent<FirstPersonController>().enabled = false;
    GameObject.Find("GameManager").GetComponent<GameManager>().EndGame();
  }

  void UpdateHealthBar() {
    healthBar.value = PercentHealth();
  }

  float PercentHealth() {
    return Mathf.Clamp01(health / 100f);
  }

  public void IncrementScore() {
    score++;
    scoreText.text = "Score: " + score;
    finalScoreText.text = "Score: " + score;
  }

}
