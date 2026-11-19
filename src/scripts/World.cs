using Godot;
using System;

public partial class World : Node2D
{
	private Character _player;
	private Camera2D _camera;
	public override void _Ready()
	{
		_player = GetNode<Character>("Player");
		_camera = GetNode<Camera2D>("Camera");
	}

	public override void _Process(double delta)
	{
		//verifica se a posição do player está maior que a da câmera, caso sim a câmera fica na posição do player
		if (_player.Position.X > _camera.Position.X)
		{
			Vector2 camPos =  _camera.Position;
			camPos.X = _player.Position.X;
			_camera.Position = camPos;
		}
		
	}
}
