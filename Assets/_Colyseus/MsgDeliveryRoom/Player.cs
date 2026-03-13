// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

namespace Rubik.MsgDelivery {
	public partial class Player : Schema {
		[Type(0, "string")]
		public string SessionId = default(string);

		[Type(1, "string")]
		public string UserID = default(string);
	}
}
