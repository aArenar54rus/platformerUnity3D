using UnityEngine;
using System.Collections;

public class characterControllerPlayerOne : MonoBehaviour {
	//клавиши управления
	public KeyCode left = KeyCode.A;
	public KeyCode right = KeyCode.D;
	public KeyCode jump = KeyCode.Space;
	//макс скорость и сила прыжка
	public float speed = 2f;
	public float jumpForce = 700f;
	//выбор камеры
	public Camera mainCamera;
	// на старте, персонаж смотрит вправо?
	public bool facingRight = true;
	private Vector3 theScale;
	private Rigidbody2D body;
	private Vector3 pos;
	private float h;
	//Для реализации параметра жизней
	public int maxLife = 100;
	public int life = 100;
	//параметр "Выносливости" - энергия, максимальная энергия и 2 переменные времени для 
	//создания регенерации энергии
	public int maxEnergy = 100;
	public int energy = 50;
	public float TimeDelay = 1;
	public float TimeDelayEnergy;
	//работа с определением земли под ногами
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	//ГУИ. Проще говоря, интерфейс.
	void OnGUI(){
		GUI.Box (new Rect (10, 10, 150, 30), "Жизни: " + life + " / " + maxLife);
		GUI.Box (new Rect (10, 40, 150, 30), "Энергия: " + energy + " / " + maxEnergy);
	}

	void Awake () 
	{
		GetComponent<Rigidbody2D> ().freezeRotation = true; //запрет на вращение тела
		body = GetComponent<Rigidbody2D>();
		theScale = transform.localScale; 
	}

	void FixedUpdate()
	{
		//Определение земли. Я сам не до конца понимаю расчет
		//Но это лучшая и самая правильная реализация из всех, что я смог найти.
		//придумать свое более, чем через счетчик, у меня не вышло.
		//GroundCheck - это положение ног. У объекта Игрока ты можешь увидеть подъобъект groundCheck
		//в нем координаты ног персонажа.
		//whatIsGround - каждая платформа имеет слой Layer=Platform. В итоге, всё, что имеет этот слой есть платформа
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		// Движение тела в зависимости от нажатой клавиши (смотри переменную h)
		//имеется баг с инерцией. Возможно, стоит упростить код перемещения
		body.AddForce(Vector2.right * h * speed * body.mass * 150);		
		
		if(Mathf.Abs(body.velocity.x) > speed) // если скорость тела превышает установленную, то выравниваем ее
			body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed, body.velocity.y);

		// переносим позицию из мировых координат в экранные
		pos = mainCamera.WorldToScreenPoint(transform.position);
		
		if(h == 0) LookAtCursor();

		//восстановление энергии через временной промежуток
		if (energy < maxEnergy) {
			TimeDelayEnergy += Time.maximumDeltaTime;
			if (TimeDelayEnergy >= TimeDelay){
				energy++;
				TimeDelayEnergy = 0;
			}
		}
	}
	
	void Update () 
	{
		//понятие жизней. Ноль жизней - загрузка уровня с нуля. Надо переделать на загрузку меню выбора
		if (life == 0) {
			Application.LoadLevel(Application.loadedLevel);
		}

		//вычисление нажатой клавиши для передвижения (см выше), смена направления движения в зависимости
		//от знаков +- Гениально, не правда ли? И кода меньше.
		if(Input.GetKey(left)) h = -1; 
		else if(Input.GetKey(right)) h = 1; 
		else h = 0;

		//Контроль прыжка (я заебался искать и придумывать его управление, в итоге, я переписал
		//старое управление и внес строку с уборкой ускорения, что есть ниже по гайду
		//баг исправлен. Попробуй сам
		if (Input.GetKeyDown (jump) && (energy >= 10) && grounded) {
			energy -= 10;
			GetComponent<Rigidbody2D>().AddForce (new Vector2 (0f, jumpForce));	//сам прыжок
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);	//убираем ускорение с наклонных поверхностей
		}


		if(!Input.GetMouseButton(0)) // если ЛКМ (стрельба) не нажата, разворот по вектору движения
		{
			if(h > 0 && !facingRight) Flip(); 
			else if(h < 0 && facingRight) Flip();
		}
		else 
		{
			LookAtCursor();
		}
	}

	// разворот относительно позиции курсора
	void LookAtCursor()
	{
		if (Input.mousePosition.x < pos.x && facingRight) {
			Flip();
		} 
		else if (Input.mousePosition.x > pos.x && !facingRight) {
			Flip();
		}
	}

	// отразить по горизонтали
	void Flip() 
	{
		facingRight = !facingRight;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//для расчет урона от других объектов
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "trap")
		{
			life = 0;
			Debug.Log("Жизни = 0");
		}
		if (col.gameObject.tag == "zombie_enemy")
		{
			life -= 5;
			Debug.Log("Потеря 5 жизней");
		}
	}
}