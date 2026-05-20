using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isMirrored;
    public GroundDetection GroundDet;
    public float speed = 100f;
    public float jumpForce = 50f;
    private bool faceright = true;
    private float moveX;
    [SerializeField] private Rigidbody2D rb;
    private bool onGround;

    // Улучшенная система прыжка
    private bool shouldJump;
    private bool canJump = true; // Может ли персонаж прыгать
    public float jumpCooldown = 0.2f; // Небольшая задержка между прыжками

    void Start()
    {
        // Поиск компонента GroundDetection
        if (GroundDet == null)
            GroundDet = FindObjectOfType<GroundDetection>();

        if (GroundDet != null)
            GroundDet.OnGroundChanged += ChangeGround;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Проверяем, есть ли у объекта Rigidbody2D
        if (rb == null)
            Debug.LogError("Rigidbody2D не найден на объекте " + gameObject.name);
    }

    private void OnDestroy()
    {
        if (GroundDet != null)
            GroundDet.OnGroundChanged -= ChangeGround;
    }

    private void ChangeGround(bool isOnGround)
    {
        onGround = isOnGround;
        Debug.Log("На земле: " + isOnGround); // Для отладки

        // Когда касаемся земли, разрешаем прыжок
        if (isOnGround)
        {
            canJump = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Debug.Log("Тестовый прыжок!");
        }
    }

    private void FixedUpdate()
    {
        // Получаем направление движения
        moveX = GetDirection();

        // Обрабатываем направление спрайта
        HandleSpriteDirection(moveX);

        // Обрабатываем движение
        HandleMovement(moveX);

        // Обрабатываем прыжок, если есть запрос
        if (shouldJump)
        {
            PerformJump();
            shouldJump = false;
        }
    }

    private void HandleMovement(float moveX)
    {
        if (rb == null) return;

        // Сохраняем текущую вертикальную скорость
        float vVelocity = rb.velocity.y;

        // Вычисляем горизонтальную скорость
        float hVelocity = moveX * speed;

        // Применяем новую скорость
        rb.velocity = new Vector2(hVelocity, vVelocity);
    }

    private void PerformJump()
    {
        if (rb == null) return;

        Debug.Log("PerformJump вызван! Сила прыжка: " + jumpForce);

        // Сохраняем текущую горизонтальную скорость
        float currentHorizontalSpeed = rb.velocity.x;

        // ВАЖНО: Обнуляем вертикальную скорость перед прыжком для более стабильного прыжка
        // Это гарантирует, что прыжок всегда будет одинаковой высоты
        rb.velocity = new Vector2(currentHorizontalSpeed, 0f);

        // Применяем силу прыжка
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        // Альтернативный вариант через velocity:
        // rb.velocity = new Vector2(currentHorizontalSpeed, jumpForce);

        // Запрещаем прыгать снова
        canJump = false;

        Debug.Log("Прыжок выполнен! Текущая скорость: " + rb.velocity);

        // Запускаем корутину для сброса флага прыжка (на всякий случай)
        StartCoroutine(JumpCooldownRoutine());
    }

    private IEnumerator JumpCooldownRoutine()
    {
        yield return new WaitForSeconds(jumpCooldown);
        // Не сбрасываем canJump здесь, так как это должно происходить при касании земли
    }

    private void HandleSpriteDirection(float moveX)
    {
        if (moveX > 0 && !faceright)
        {
            Flip(moveX);
        }
        else if (moveX < 0 && faceright)
        {
            Flip(moveX);
        }
    }

    private float GetDirection()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        if (isMirrored)
        {
            return direction * -1;
        }
        else
        {
            return direction;
        }
    }

    private void Flip(float direction)
    {
        faceright = !faceright;

        if (!isMirrored)
        {
            if (direction < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (direction > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (direction < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (direction > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, коснулись ли мы земли
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Коснулся земли!");
            canJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Дополнительная проверка, если персонаж стоит на земле
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (!canJump)
            {
                Debug.Log("Стою на земле - можно прыгать");
                canJump = true;
            }
        }
    }
}