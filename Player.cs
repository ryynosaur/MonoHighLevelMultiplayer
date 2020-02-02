using Godot;

public class Player : KinematicBody2D
{
	[Export] public int Speed = 200;

	private Vector2 velocity = new Vector2();

	private Label NameLabel { get; set; }

	[Puppet]
	public Vector2 PuppetPosition { get; set; }
	[Puppet]
	public Vector2 PuppetVelocity { get; set; }

	public void GetInput()
	{
		velocity = new Vector2();

		if (Input.IsActionPressed("right"))
			velocity.x += 1;

		if (Input.IsActionPressed("left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("down"))
			velocity.y += 1;

		if (Input.IsActionPressed("up"))
			velocity.y -= 1;

		velocity = velocity.Normalized() * Speed;

		Rset(nameof(PuppetPosition), Position);
		Rset(nameof(PuppetVelocity), velocity);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsNetworkMaster())
		{
			GetInput();
		}
		else
		{
			Position = PuppetPosition;
			velocity = PuppetVelocity;
		}
		
		velocity = MoveAndSlide(velocity);

		if (!IsNetworkMaster())
		{
			PuppetPosition = Position;
		}
	}

	public void SetPlayerName(string name)
	{
		NameLabel = (Label)GetNode("Label");

		PuppetPosition = Position;
		PuppetVelocity = velocity;

		NameLabel.Text = name;
	}
}
