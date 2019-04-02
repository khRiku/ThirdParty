using System;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x0200004B RID: 75
	public class DOTweenSettings : ScriptableObject
	{
		// Token: 0x04000123 RID: 291
		public const string AssetName = "DOTweenSettings";

		// Token: 0x04000124 RID: 292
		public bool useSafeMode = true;

		// Token: 0x04000125 RID: 293
		public float timeScale = 1f;

		// Token: 0x04000126 RID: 294
		public bool useSmoothDeltaTime;

		// Token: 0x04000127 RID: 295
		public float maxSmoothUnscaledTime = 0.15f;

		// Token: 0x04000128 RID: 296
		public RewindCallbackMode rewindCallbackMode;

		// Token: 0x04000129 RID: 297
		public bool showUnityEditorReport;

		// Token: 0x0400012A RID: 298
		public LogBehaviour logBehaviour;

		// Token: 0x0400012B RID: 299
		public bool drawGizmos = true;

		// Token: 0x0400012C RID: 300
		public bool defaultRecyclable;

		// Token: 0x0400012D RID: 301
		public AutoPlay defaultAutoPlay = AutoPlay.All;

		// Token: 0x0400012E RID: 302
		public UpdateType defaultUpdateType;

		// Token: 0x0400012F RID: 303
		public bool defaultTimeScaleIndependent;

		// Token: 0x04000130 RID: 304
		public Ease defaultEaseType = Ease.OutQuad;

		// Token: 0x04000131 RID: 305
		public float defaultEaseOvershootOrAmplitude = 1.70158f;

		// Token: 0x04000132 RID: 306
		public float defaultEasePeriod;

		// Token: 0x04000133 RID: 307
		public bool defaultAutoKill = true;

		// Token: 0x04000134 RID: 308
		public LoopType defaultLoopType;

		// Token: 0x04000135 RID: 309
		public DOTweenSettings.SettingsLocation storeSettingsLocation;

		// Token: 0x04000136 RID: 310
		public DOTweenSettings.ModulesSetup modules = new DOTweenSettings.ModulesSetup();

		// Token: 0x04000137 RID: 311
		public bool showPlayingTweens;

		// Token: 0x04000138 RID: 312
		public bool showPausedTweens;

		// Token: 0x020000AD RID: 173
		public enum SettingsLocation
		{
			// Token: 0x04000212 RID: 530
			AssetsDirectory,
			// Token: 0x04000213 RID: 531
			DOTweenDirectory,
			// Token: 0x04000214 RID: 532
			DemigiantDirectory
		}

		// Token: 0x020000AE RID: 174
		[Serializable]
		public class ModulesSetup
		{
			// Token: 0x04000215 RID: 533
			public bool showPanel;

			// Token: 0x04000216 RID: 534
			public bool audioEnabled = true;

			// Token: 0x04000217 RID: 535
			public bool physicsEnabled = true;

			// Token: 0x04000218 RID: 536
			public bool physics2DEnabled = true;

			// Token: 0x04000219 RID: 537
			public bool spriteEnabled = true;

			// Token: 0x0400021A RID: 538
			public bool uiEnabled = true;

			// Token: 0x0400021B RID: 539
			public bool textMeshProEnabled;

			// Token: 0x0400021C RID: 540
			public bool tk2DEnabled;
		}
	}
}
