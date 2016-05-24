using System;
using UnityEngine;

namespace RTS
{

	[Serializable]
	public struct Action {
		public string name;
		public GameObject unitPrefab;
		public ActionType actionType;
	}

	[Serializable]
	public enum ActionType {
		BuildUnit, Upgrade
	}
}

