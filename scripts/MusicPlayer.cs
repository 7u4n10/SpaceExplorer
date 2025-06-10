using Godot;
using System;
using System.Collections.Generic;

public partial class MusicPlayer : AudioStreamPlayer3D
{
	private List<AudioStream> _tracks = new();
	private Random _rng = new();

	public override void _Ready()
	{
		_tracks.Add(ResourceLoader.Load<AudioStream>("res://musics/Moonset(chosic.com).mp3"));
		_tracks.Add(ResourceLoader.Load<AudioStream>("res://musics/scott-buckley-jul(chosic.com).mp3"));
		_tracks.Add(ResourceLoader.Load<AudioStream>("res://musics/Sunset-Landscape(chosic.com).mp3"));

		PlayRandomTrack();
	}

	public override void _Process(double delta)
	{
		if (!Playing)
		{
			PlayRandomTrack();
		}
	}

	private void PlayRandomTrack()
	{
		int index = _rng.Next(_tracks.Count);
		Stream = _tracks[index];
		Play();
	}
}
