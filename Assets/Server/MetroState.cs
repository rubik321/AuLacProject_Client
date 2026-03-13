// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.28
// 

using Colyseus.Schema;

public partial class MetroState : Schema {
	[Type(0, "map", typeof(MapSchema<MetroPlayer>))]
	public MapSchema<MetroPlayer> players = new MapSchema<MetroPlayer>();
}

