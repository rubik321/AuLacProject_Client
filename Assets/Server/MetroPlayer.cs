// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.28
// 

using Colyseus.Schema;

public partial class MetroPlayer : Schema {
	[Type(0, "number")]
	public float x = default(float);

	[Type(1, "number")]
	public float y = default(float);

	[Type(2, "number")]
	public float dx = default(float);

	[Type(3, "number")]
	public float dy = default(float);

	[Type(4, "string")]
	public string action = default(string);

	[Type(5, "boolean")]
	public bool isASWD = default(bool);

	[Type(6, "boolean")]
	public bool isRight = default(bool);
}

